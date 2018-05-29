using Microsoft.Extensions.Options;
using paypart_category_gateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace paypart_category_gateway.Services
{
    public class Utility
    {
        private readonly IOptions<Settings> settings;
        public Utility(IOptions<Settings> _settings)
        {
            settings = _settings;
        }
        public async Task<IEnumerable<BillerCategory>> getBillers(string key, CancellationToken ctx)
        {
            IEnumerable<BillerCategory> billers = new List<BillerCategory>();
            try
            {
                //billers = await 
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return billers;
        }
    }
}
