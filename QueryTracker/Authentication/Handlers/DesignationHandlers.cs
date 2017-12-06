using Microsoft.AspNetCore.Authorization;
using QueryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Authentication.Handlers
{
	public class DesignationHandler : AuthorizationHandler<UserDesignationRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserDesignationRequirement requirement)
		{
			if (!context.User.HasClaim(c => c.Type == ClaimsConstants.Designation))
			{
				return Task.CompletedTask;
			}

			if (requirement.IsAccessAllowedToUser((DesignationEnum)(int.Parse(context.User.Claims.First(c => c.Type == ClaimsConstants.Designation).Value))))
			{
				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}
}
