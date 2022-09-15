using GenericStoreApp.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;


namespace GenericStoreApp.Views.Shared.Components.ClerkList;
[ViewComponent(Name = "ClientList")]
[Authorize(Roles = "Admin")]
public class ClientListViewComponent : ViewComponent
{
    private readonly ApplicationDbContext db;

    public ClientListViewComponent(ApplicationDbContext context) => db = context;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var items = await GetItemsAsync();
            return View(items);
    }

    private Task<List<IdentityUser>> GetItemsAsync()

    {
        Console.WriteLine(db!.UserRoles);

        var roleId = db!.Roles!.FirstOrDefault(r => r.NormalizedName == "CLIENTS")?.Id;

        var userIds = db!.UserRoles!.Where(u => u.RoleId == roleId).ToListAsync().Result;

        List<string> clerkIds = new List<string>();

        foreach (var user in userIds)
        {
            clerkIds.Add(user.UserId);
        }

        return db!.Users!.Where(u => clerkIds.Any(id => id == u.Id)).ToListAsync();
    }
}