using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Technology_Shop.Models;

public class SameUserHandler : AuthorizationHandler<SameUserRequirement>
{
	protected override Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		SameUserRequirement requirement)
	{
		var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (!string.IsNullOrEmpty(userIdClaim))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}