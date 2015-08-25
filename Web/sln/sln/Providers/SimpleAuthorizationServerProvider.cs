using Michal.Project.Dal;
using Michal.Project.Helper;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            using ( var dBContext = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(dBContext);
                var user = await userManager.FindAsync(context.UserName, context.Password);
             
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
               
                //identity.AddClaim(new Claim(CustomClaimTypes.JobTitle, jobTitle));
                //identity.AddClaim(new Claim(CustomClaimTypes.JobType, ((int)jobType).ToString()));

                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.GroupSid, user.Organization_OrgId.HasValue ? user.Organization_OrgId.Value.ToString() : General.OrgIDWWW));
                identity.AddClaim(new Claim(ClaimTypes.SerialNumber, String.IsNullOrEmpty(user.EmpId) ? "אן מספר עובד" : user.EmpId));
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.FirstName + " " + user.LastName));
            } 
          

            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);

        }
    }
}