using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudentPortal.Models;

namespace StudentPortal.Data;

public sealed class ApplicationUserClaimsPrincipalFactory
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public ApplicationUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var existingName = identity.FindFirst(Options.ClaimsIdentity.UserNameClaimType);
        if (existingName is not null)
        {
            identity.RemoveClaim(existingName);
        }

        identity.AddClaim(new Claim(
            Options.ClaimsIdentity.UserNameClaimType,
            $"{user.FirstName} {user.LastName}".Trim()));
        return identity;
    }
}
