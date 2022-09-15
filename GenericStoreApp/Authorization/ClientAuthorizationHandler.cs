using GenericStoreApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GenericStoreApp.Authorization;

public class ClientAuthorizationHandler :
AuthorizationHandler<OperationAuthorizationRequirement, Product>
{
protected override Task
    HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Product resource)
{
    if (context.User == null || resource == null)
    {
        return Task.CompletedTask;
    }
    
    // If not asking for AddToCart
    if (requirement.Name != Constants.AddToCartOperationName

       )
    {
        return Task.CompletedTask;
    }

    
    if (context.User.IsInRole(Constants.ClientsRole))
    {
        context.Succeed(requirement);
    }

    return Task.CompletedTask;
}
}
