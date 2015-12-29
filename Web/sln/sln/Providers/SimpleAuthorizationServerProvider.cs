using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
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

        //public override  Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        //{
        //    context.Validated();
        //    return Task.FromResult<object>(null);
        //}
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            //using (AuthRepository _repo = new AuthRepository())
            using (var dbcontext = new ApplicationDbContext())
            {
                var _repo = new UserRepository(dbcontext);
                client = _repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Michal.Project.Helper.HelperSecurity.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }


        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{

        //    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
        //    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

        //    using (var dBContext = new ApplicationDbContext())
        //    {
        //        var userManager = new ApplicationUserManager(dBContext);
        //        var user = await userManager.FindAsync(context.UserName, context.Password);

        //        if (user == null)
        //        {
        //            context.SetError("invalid_grant", "The user name or password is incorrect.");
        //            return;
        //        }

        //        //identity.AddClaim(new Claim(CustomClaimTypes.JobTitle, jobTitle));
        //        //identity.AddClaim(new Claim(CustomClaimTypes.JobType, ((int)jobType).ToString()));

        //        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        //        identity.AddClaim(new Claim(ClaimTypes.GroupSid, user.Organization_OrgId.HasValue ? user.Organization_OrgId.Value.ToString() : General.OrgIDWWW));
        //        identity.AddClaim(new Claim(ClaimTypes.SerialNumber, String.IsNullOrEmpty(user.EmpId) ? "אן מספר עובד" : user.EmpId));
        //        identity.AddClaim(new Claim(ClaimTypes.Surname, user.FirstName + " " + user.LastName));
        //    }

        //    context.Validated(identity);

        //}


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            Organization organization=null;
            if (allowedOrigin == null) allowedOrigin = "*";
            ApplicationUser user = null;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                using (var dBContext = new ApplicationDbContext())
                {  MemeryCacheDataService cache = new MemeryCacheDataService();
                    organization= cache.GetOrgEntity(dBContext); //await context.Organization.ToListAsync();
                    var userManager = new ApplicationUserManager(dBContext);
                     user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            HelperSecurity.SetClaims(identity,user, organization, JobType.Runner, Helper.JobTitle.DeliveryBoy);
            //identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    { 
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}