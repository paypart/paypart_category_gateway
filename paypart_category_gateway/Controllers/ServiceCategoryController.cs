using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using paypart_category_gateway.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using paypart_category_gateway.Models;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace paypart_category_gateway.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/servicecategory")]
    public class ServiceCategoryController : Controller
    {
        private readonly IBillerCategorySqlServerRepository billercategorySqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        public ServiceCategoryController(IOptions<Settings> _settings,
          IBillerCategorySqlServerRepository _billercategorySqlRepo, IDistributedCache _cache)
        {
            settings = _settings;
            billercategorySqlRepo = _billercategorySqlRepo;
            cache = _cache;
        }
        [HttpGet("getallservicecategories")]
        [ProducesResponseType(typeof(ServiceCategory), 200)]
        [ProducesResponseType(typeof(ServiceCategoryError), 400)]
        [ProducesResponseType(typeof(ServiceCategoryError), 500)]
        public async Task<IActionResult> getallservicecategories()
        {
            List<ServiceCategory> servicecategories = null;
            ServiceCategoryError e = new ServiceCategoryError();

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            Redis redis = new Redis(settings, cache);
            string key = "all_service_categories";

            //check redis cache for details
            try
            {
                servicecategories = await redis.getservicecategories(key, cts.Token);

                if (servicecategories != null && servicecategories.Count > 0)
                {
                    return CreatedAtAction("getallservicecategories", servicecategories);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Get billers from Mongo
            try
            {
                //billercategories = await billercategoryMongoRepo.GetAllBillerCategories();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Get Billers from Sql
            try
            {
                servicecategories = await billercategorySqlRepo.GetAllServiceCategories();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Write to Redis
            try
            {
                if (servicecategories != null && servicecategories.Count > 0)
                    await redis.setservicecategories(key, servicecategories, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return CreatedAtAction("getallservicecategories", servicecategories);
        }

        [HttpPost("addservicecategory")]
        [ProducesResponseType(typeof(ServiceCategory), 200)]
        [ProducesResponseType(typeof(ServiceCategoryError), 400)]
        [ProducesResponseType(typeof(ServiceCategoryError), 500)]
        public async Task<IActionResult> addservicecategory([FromBody]ServiceCategory servicecategory)
        {
            ServiceCategory _servicecategory = null;
            List<ServiceCategory> servicecategories = null;
            ServiceCategoryError e = new ServiceCategoryError();
            Redis redis = new Redis(settings, cache);

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            servicecategory.created_on = DateTime.Now;


            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<ServiceCategoryError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            //Add to mongo
            try
            {
                //_billercategory = await billercategoryMongoRepo.AddBillerCategory(billercategory);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Add to sql server
            try
            {
                if (servicecategory.id == 0)
                {
                    _servicecategory = await billercategorySqlRepo.AddServiceCategory(servicecategory);
                }
                else if (servicecategory.id > 0)
                {
                    _servicecategory = await billercategorySqlRepo.UpdateServiceCategory(servicecategory);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }

            //Write to Redis
            try
            {
                string key = "all_service_category:" + _servicecategory.id;

                if (_servicecategory != null)
                    await redis.setservicecategory(key, _servicecategory, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return CreatedAtAction("addservicecategory", _servicecategory);

        }
    }
}