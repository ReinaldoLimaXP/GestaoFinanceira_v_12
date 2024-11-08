using GestaoFinanceira_v_1.Components.Account.Pages;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;


namespace GestaoFinanceira_v_1.Data
{
    public class IdentityExtensions : UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        public IdentityExtensions(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(nameof(ApplicationUser.Nome_Usuario), value: user.Nome_Usuario));
            identity.AddClaim(new Claim(nameof(ApplicationUser.Id_funcionario), value:  user.Id_funcionario.ToString()));     
            return identity;

        }
    }
}