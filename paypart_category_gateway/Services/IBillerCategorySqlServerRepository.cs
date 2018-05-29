using System.Collections.Generic;
using paypart_category_gateway.Models;
using System.Threading.Tasks;

namespace paypart_category_gateway.Services
{
    public interface IBillerCategorySqlServerRepository
    {
        Task<List<BillerCategory>> GetAllBillerCategories();
        Task<BillerCategory> GetBillerCategory(int id);
        Task<List<BillerCategory>> GetBillerCategoryByTitle(string title);
        Task<BillerCategory> AddBillerCategory(BillerCategory item);
        Task<ServiceCategory> GetServiceCategory(int id);
        Task<ServiceCategory> AddServiceCategory(ServiceCategory item);
        Task<List<ServiceCategory>> GetAllServiceCategories();
        Task<BillerCategory> UpdateBillerCategory(BillerCategory billercategory);
        Task<ServiceCategory> UpdateServiceCategory(ServiceCategory servicecategory);
        //Task<DeleteResult> RemoveBiller(string id);
        //Task<UpdateResult> UpdateBiller(string id, string title);

        // demo interface - full document update
        //Task<ReplaceOneResult> UpdateBiller(string id, Biller item);

        // should be used with high cautious, only in relation with demo setup
        //Task<DeleteResult> RemoveAllBillers();

    }
}
