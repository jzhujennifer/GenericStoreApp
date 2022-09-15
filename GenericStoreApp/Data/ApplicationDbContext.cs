using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GenericStoreApp.Models;

namespace GenericStoreApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GenericStoreApp.Models.Product>? Product { get; set; }
        public DbSet<GenericStoreApp.Models.Order>? Order { get; set; }
        public DbSet<GenericStoreApp.Models.ProductSale>? ProductSale { get; set; }
        public DbSet<GenericStoreApp.Models.ContactUs>? ContactUs { get; set; }
    }
}