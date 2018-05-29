using Microsoft.EntityFrameworkCore;
using paypart_category_gateway.Models;

namespace paypart_category_gateway.Services
{
    public class BillerCategorySqlServerContext : DbContext
    {
        public BillerCategorySqlServerContext(DbContextOptions<BillerCategorySqlServerContext> options) : base(options)
        {

        }

        public DbSet<BillerCategory> BillersCategory { get; set; }
        public DbSet<ServiceCategory> ServicesCategory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillerCategory>().ToTable("BillerCategory");
            modelBuilder.Entity<ServiceCategory>().ToTable("ServiceCategory");

        }
    }
}
