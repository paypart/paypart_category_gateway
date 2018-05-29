using Microsoft.Extensions.Options;
using MongoDB.Driver;
using paypart_category_gateway.Models;

namespace paypart_category_gateway.Services
{
    public class BillerCategoryMongoContext
    {
        private readonly IMongoDatabase _database = null;

        public BillerCategoryMongoContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.connectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.database);
        }

        public IMongoCollection<BillerCategory> BillerCategories
        {
            get
            {
                return _database.GetCollection<BillerCategory>("billercategories");
            }
        }
    }
}
