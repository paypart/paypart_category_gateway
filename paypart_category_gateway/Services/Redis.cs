using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using paypart_category_gateway.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;

namespace paypart_category_gateway.Services
{
    public class Redis
    {
        IOptions<Settings> settings;
        IDistributedCache redis;
        public delegate void SetBiller(string key, BillerCategory category);
        public delegate void SetBillers(string key, IEnumerable<BillerCategory> categories);

        public Redis(IOptions<Settings> _settings, IDistributedCache _redis)
        {
            settings = _settings;
            redis = _redis;
        }
        public async Task<BillerCategory> getbillercategory(string key, CancellationToken ctx)
        {
            BillerCategory categories = new BillerCategory();
            try
            {
                var category = await redis.GetStringAsync(key, ctx);
                if (!string.IsNullOrEmpty(category))
                    categories = JsonHelper.fromJson<BillerCategory>(category);
            }
            catch (Exception)
            {

            }
            return categories;
        }
        public async Task<ServiceCategory> getservicecategory(string key, CancellationToken ctx)
        {
            ServiceCategory categories = new ServiceCategory();
            try
            {
                var category = await redis.GetStringAsync(key, ctx);
                if (!string.IsNullOrEmpty(category))
                    categories = JsonHelper.fromJson<ServiceCategory>(category);
            }
            catch (Exception)
            {

            }
            return categories;
        }
        public async Task<List<BillerCategory>> getbillercategories(string key, CancellationToken ctx)
        {
            List<BillerCategory> categories = new List<BillerCategory>();
            try
            {
                var category = await redis.GetStringAsync(key, ctx);
                categories = JsonHelper.fromJson<List<BillerCategory>>(category);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return categories;
        }

        public async Task<List<ServiceCategory>> getservicecategories(string key, CancellationToken ctx)
        {
            List<ServiceCategory> categories = new List<ServiceCategory>();
            try
            {
                var category = await redis.GetStringAsync(key, ctx);
                categories = JsonHelper.fromJson<List<ServiceCategory>>(category);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return categories;
        }

        public async Task setbillercategory(string key, BillerCategory categories, CancellationToken cts)
        {
            try
            {
                var category = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(category))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(categories);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }

        }

        public async Task setservicecategory(string key, ServiceCategory categories,CancellationToken cts)
        {
            try
            {
                var category = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(category))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(categories);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }

        }
        public async Task setbillercategoryAsync(string key, BillerCategory categories, CancellationToken cts)
        {
            try
            {
                var category = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(category))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(categories);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception ex)
            {

            }

        }

        public async Task setservicecategories(string key, List<ServiceCategory> categories, CancellationToken cts)
        {
            try
            {
                var category = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(category))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(categories);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }

        public async Task setbillercategories(string key, List<BillerCategory> categories, CancellationToken cts)
        {
            try
            {
                var category = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(category))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(categories);
                
                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }
    }
}
