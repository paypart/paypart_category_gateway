using Microsoft.EntityFrameworkCore;
using paypart_category_gateway.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_category_gateway.Services
{
    public class BillerCategorySqlServerRepository : IBillerCategorySqlServerRepository
    {
        private readonly BillerCategorySqlServerContext _context = null;

        public BillerCategorySqlServerRepository(BillerCategorySqlServerContext context)
        {
            _context = context;
        }

        public async Task<List<BillerCategory>> GetAllBillerCategories()
        {
            //List<BillerCategory> bbb = _context.BillersCategory.ToList();
            return await _context.BillersCategory.ToListAsync();
        }

        public async Task<BillerCategory> GetBillerCategory(int id)
        {
            return await _context.BillersCategory.Where(c => c._id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<ServiceCategory>> GetAllServiceCategories()
        {
            //List<BillerCategory> bbb = _context.BillersCategory.ToList();
            return await _context.ServicesCategory.ToListAsync();
        }

        public async Task<ServiceCategory> GetServiceCategory(int id)
        {
            return await _context.ServicesCategory.Where(c => c.id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<BillerCategory>> GetBillerCategoryByTitle(string title)
        {
            return await _context.BillersCategory.Where(c => c.title == title)
                                 .ToListAsync();
        }

        public async Task<BillerCategory> AddBillerCategory(BillerCategory item)
        {
            await _context.BillersCategory.AddAsync(item);
            await _context.SaveChangesAsync();
            return await GetBillerCategory(item._id);
        }

        public async Task<ServiceCategory> AddServiceCategory(ServiceCategory item)
        {
            await _context.ServicesCategory.AddAsync(item);
            await _context.SaveChangesAsync();
            return await GetServiceCategory(item.id);
        }
        public async Task<BillerCategory> UpdateBillerCategory(BillerCategory billercategory)
        {
            BillerCategory bc = await GetBillerCategory(billercategory._id);
            bc.title = billercategory.title;
            bc.status = billercategory.status;

            await _context.SaveChangesAsync();
            return bc;
        }

        public async Task<ServiceCategory> UpdateServiceCategory(ServiceCategory servicecategory)
        {
            ServiceCategory sc = await GetServiceCategory(servicecategory.id);
            sc.title = servicecategory.title;
            sc.status = servicecategory.status;

            await _context.SaveChangesAsync();
            return sc;
        }
        //public async Task<DeleteResult> RemoveBiller(string id)
        //{
        //    return await _context.Billers.Remove(
        //                 Builders<Biller>.Filter.Eq(s => s._id, id));
        //}

        //public async Task<UpdateResult> UpdateBiller(string id, string title)
        //{
        //    var filter = Builders<Biller>.Filter.Eq(s => s._id.ToString(), id);
        //    var update = Builders<Biller>.Update
        //                        .Set(s => s.title, title)
        //                        .CurrentDate(s => s.createdOn);
        //    return await _context.Billers.UpdateOneAsync(filter, update);
        //}

        //public async Task<ReplaceOneResult> UpdateBiller(string id, Biller item)
        //{
        //    return await _context.Billers
        //                         .ReplaceOneAsync(n => n._id.Equals(id)
        //                                             , item
        //                                             , new UpdateOptions { IsUpsert = true });
        //}

        //public async Task<DeleteResult> RemoveAllBillers()
        //{
        //    return await _context.Billers.DeleteManyAsync(new BsonDocument());
        //}
    }
}
