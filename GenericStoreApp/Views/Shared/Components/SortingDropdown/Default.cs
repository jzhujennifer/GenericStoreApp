using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenericStoreApp.Data;

namespace GenericStoreApp.Views.Shared.Components.SortingDropdown;
[ViewComponent(Name = "SortingDropdown")]
[AllowAnonymous]
public class SortingDropDown : ViewComponent
{
    private readonly ApplicationDbContext db;

    public SortingDropDown(ApplicationDbContext context) => db = context;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        List<string> categories = new List<string> { "Sort - Category" };
        var items = await GetItemsAsync();
        categories.AddRange(items);

        
        return View(categories);
    }

    private Task<List<string>> GetItemsAsync()
    {
        return db!.Product!.Select(p => p.Category).Distinct().ToListAsync();

    }
}