using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using paypart_category_gateway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace paypart_category_gateway.Services
{
    public class BillerCategoryMongoRepository : IBillerCategoryMongoRepository
    {
        private readonly BillerCategoryMongoContext _context = null;

        public BillerCategoryMongoRepository(IOptions<Settings> settings)
        {
            _context = new BillerCategoryMongoContext(settings);
        }

        public async Task<List<BillerCategory>> GetAllBillerCategories()
        {
            return await _context.BillerCategories.Find(_ => true).ToListAsync();
        }

        public async Task<BillerCategory> GetBillerCategory(int id)
        {
            var filter = Builders<BillerCategory>.Filter.Eq("_id", id);
            return await _context.BillerCategories
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
        }
        public async Task<List<BillerCategory>> GetBillerCategoryByTitle(string title)
        {
            var filter = Builders<BillerCategory>.Filter.Eq(b => b.title, title);
            return await _context.BillerCategories
                                 .Find(filter)
                                 .ToListAsync();
        }

        public async Task<BillerCategory> AddBillerCategory(BillerCategory item)
        {
            await _context.BillerCategories.InsertOneAsync(item);
            return await GetBillerCategory(item._id);
        }

        public async Task<DeleteResult> RemoveBillerCategory(int id)
        {
            return await _context.BillerCategories.DeleteOneAsync(
                         Builders<BillerCategory>.Filter.Eq(s => s._id, id));
        }

        public async Task<UpdateResult> UpdateBillerCategory(string id, string title)
        {
            var filter = Builders<BillerCategory>.Filter.Eq(s => s._id.ToString(), id);
            var update = Builders<BillerCategory>.Update
                                .Set(s => s.title, title)
                                .CurrentDate(s => s.created_on);
            return await _context.BillerCategories.UpdateOneAsync(filter, update);
        }

        public async Task<ReplaceOneResult> UpdateBillerCategory(string id, BillerCategory item)
        {
            return await _context.BillerCategories
                                 .ReplaceOneAsync(n => n._id.Equals(id)
                                                     , item
                                                     , new UpdateOptions { IsUpsert = true });
        }

        public async Task<DeleteResult> RemoveAllBillerCategories()
        {
            return await _context.BillerCategories.DeleteManyAsync(new BsonDocument());
        }

    }
}
