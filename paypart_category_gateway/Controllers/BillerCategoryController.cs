using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using paypart_category_gateway.Models;
using Microsoft.AspNetCore.Mvc;
using paypart_category_gateway.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace paypart_category_gateway.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/billercategory")]
    public class BillerCategoryController : Controller
    {
        private readonly IBillerCategoryMongoRepository billercategoryMongoRepo;
        private readonly IBillerCategorySqlServerRepository billercategorySqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        IOptions<List<nameValuePair>> nvp;

        public BillerCategoryController(IOptions<Settings> _settings, IBillerCategoryMongoRepository _billercategoryMongoRepo
            , IBillerCategorySqlServerRepository _billercategorySqlRepo, IDistributedCache _cache, IOptions<List<nameValuePair>> _nvp)
        {
            nvp = _nvp;
            settings = _settings;
            billercategoryMongoRepo = _billercategoryMongoRepo;
            billercategorySqlRepo = _billercategorySqlRepo;
            cache = _cache;
        }

        [HttpGet("getallbillercategories")]
        [ProducesResponseType(typeof(BillerCategory), 200)]
        [ProducesResponseType(typeof(BillerCategoryError), 400)]
        [ProducesResponseType(typeof(BillerCategoryError), 500)]
        public async Task<IActionResult> getallbillercategories()
        {
            List<BillerCategory> billercategories = null;
            BillerCategoryError e = new BillerCategoryError();

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            Redis redis = new Redis(settings, cache);
            string key = "all_biller_categories";

            //check redis cache for details
            try
            {
                billercategories = await redis.getbillercategories(key, cts.Token);

                if (billercategories != null && billercategories.Count > 0)
                {
                    return CreatedAtAction("getallbillercategories", billercategories);
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
                billercategories = await billercategorySqlRepo.GetAllBillerCategories();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Write to Redis
            try
            {
                if (billercategories != null && billercategories.Count > 0)
                    await redis.setbillercategories(key, billercategories, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return CreatedAtAction("getallbillercategories", billercategories);
        }


        [HttpPost("addbillercategory")]
        [ProducesResponseType(typeof(BillerCategory), 200)]
        [ProducesResponseType(typeof(BillerCategoryError), 400)]
        [ProducesResponseType(typeof(BillerCategoryError), 500)]
        public async Task<IActionResult> addbillercategory([FromBody]BillerCategory billercategory)
        {
            BillerCategory _billercategory = null;
            List<BillerCategory> billercategories = null;
            BillerCategoryError e = new BillerCategoryError();
            Redis redis = new Redis(settings, cache);

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<BillerCategoryError>();
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
            billercategory.created_on = DateTime.Now;
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
                if (billercategory._id == 0)
                {
                    _billercategory = await billercategorySqlRepo.AddBillerCategory(billercategory);
                }
                else if (billercategory._id > 0)
                {
                    _billercategory = await billercategorySqlRepo.UpdateBillerCategory(billercategory);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }

            //Write to Redis
            try
            {
                string key = "all_biller_category:" + _billercategory._id;

                if (_billercategory != null)
                    await redis.setbillercategory(key, _billercategory, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return CreatedAtAction("addbillercategory", _billercategory);

        }
    }
}