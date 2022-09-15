using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenericStoreApp.Data;

namespace GenericStoreApp.Views.Shared.Components.SortingDropdown;
[ViewComponent(Name = "ShoppingIcon")]
[AllowAnonymous]
public class ShoppingIcon : ViewComponent
{
    private readonly ApplicationDbContext db;

    public ShoppingIcon(ApplicationDbContext context) => db = context;

    public async Task<IViewComponentResult> InvokeAsync(string email)
    {
        var model = new Dictionary<string, decimal>();
        int itemCount = 0;
        decimal totalPrice = 0;
        model.Add("ItemCount", itemCount);
        model.Add("TotalPrice", totalPrice);

        var order = db.Order?.FirstOrDefault(x => x.Email == email);
        if (order == null) return View(model);

        var productSales = await db.ProductSale?.Where(x => x.OrderID == order.OrderID).ToListAsync();
        if (productSales.Any())
        {
            foreach (var productSale in productSales)
            {
                itemCount += productSale.Quantity;
                totalPrice += db.Product!.FirstOrDefault(x => x.ID == productSale.ProductID)!.Price!.Value * productSale.Quantity;
            }

            model["ItemCount"] = itemCount;
            model["TotalPrice"] = totalPrice;
            
            return View(model);
        }
        return View(model);
    }

    private Task<List<string>> GetItemsAsync()
    {
        return db!.Product!.Select(p => p.Category).Distinct().ToListAsync();

    }
}