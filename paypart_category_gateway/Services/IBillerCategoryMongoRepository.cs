using MongoDB.Driver;
using System.Collections.Generic;
using paypart_category_gateway.Models;
using System.Threading.Tasks;

namespace paypart_category_gateway.Services
{
    public interface IBillerCategoryMongoRepository
    {
        Task<List<BillerCategory>> GetAllBillerCategories();
        Task<BillerCategory> GetBillerCategory(int id);
        Task<List<BillerCategory>> GetBillerCategoryByTitle(string title);
        Task<BillerCategory> AddBillerCategory(BillerCategory item);

        Task<DeleteResult> RemoveBillerCategory(int id);
        Task<UpdateResult> UpdateBillerCategory(string id, string title);

        // demo interface - full document update
        Task<ReplaceOneResult> UpdateBillerCategory(string id, BillerCategory item);

        // should be used with high cautious, only in relation with demo setup
        Task<DeleteResult> RemoveAllBillerCategories();

    }
}
