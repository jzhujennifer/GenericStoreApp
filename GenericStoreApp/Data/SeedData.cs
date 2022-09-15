using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GenericStoreApp.Data;
using GenericStoreApp.Models;
using GenericStoreApp.Service;

// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -udl -outDir Pages\Contacts --referenceScriptLibraries

namespace GenericStoreApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw = "Pass1234!")
        {
            using (var context = new ApplicationDbContext(
                       serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@MVC.com");
                await EnsureRole(serviceProvider, adminID, Authorization.Constants.AdministratorsRole);


                var clientID = await EnsureUser(serviceProvider, testUserPw, "client@MVC.com");
                await EnsureRole(serviceProvider, clientID, Authorization.Constants.ClientsRole);




                var FakeProducts = await FakeStoreApi.GetAllFakeProductsApi();


                SeedDB(context, FakeProducts);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
            string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, List<FakeProduct> fakeProducts)


        {


            if (context.Product.Any())
            {
                return; // DB has been seeded
            }

            foreach (var fakeProduct in fakeProducts)
            {
                 context.Add(new Product
                {
                    ProductName = fakeProduct.title,
                    Price = fakeProduct.price,
                    Description = fakeProduct.description,
                    Category = fakeProduct.category,
                    ImageLink = fakeProduct.image
                });
            }


            context.SaveChanges();
        }
    }
}