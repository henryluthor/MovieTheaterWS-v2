using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// The requirement
public class AdminOrOwnerRequirement : IAuthorizationRequirement { }

// The logic handler
public class AdminOrOwnerHandler : AuthorizationHandler<AdminOrOwnerRequirement, string>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminOrOwnerRequirement requirement,
        string resourceUserId) // ID of user you want to access
    {
        var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Is the user making the request is Admin or if resource ID is his own he has access
        if (context.User.IsInRole("Admin") || currentUserId == resourceUserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

