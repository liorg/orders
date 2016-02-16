using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Microsoft.AspNet.Identity;
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

        ///http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/
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
            //http://leastprivilege.com/2013/11/15/adding-refresh-tokens-to-a-web-api-v2-authorization-server/
            context.OwinContext.Set<string>("as:client_id", context.ClientId);

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());


            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            Organization organization = null;
            if (allowedOrigin == null) allowedOrigin = "*";
            ApplicationUser user = null;
            ApplicationUserManager userManager;
            var rolesStrs = "";
            ClaimsIdentity identity; List<Claim> roles = new List<Claim>();
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            using (var dBContext = new ApplicationDbContext())
            {
                MemeryCacheDataService cache = new MemeryCacheDataService();
                organization = cache.GetOrgEntity(dBContext); //await context.Organization.ToListAsync();
                userManager = new ApplicationUserManager(dBContext);
                user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                identity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
                roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                rolesStrs = Newtonsoft.Json.JsonConvert.SerializeObject(roles.Select(x => x.Value));
            }
            HelperSecurity.SetClaims(identity, user, organization, JobType.Runner, Helper.JobTitle.DeliveryBoy);
            var currentTime = DateTime.UtcNow;
            var expiredOn = currentTime.AddMinutes(General.MAXMinutesExpiredApiToken);
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    { 
                        "userName", context.UserName
                    },
                    { 
                        "m:currentTime", currentTime.ToString("yyyy-MM-dd HH:mm:ss")
                    },
                    { 
                        "m:expiredOn", expiredOn.ToString("yyyy-MM-dd HH:mm:ss")
                    },

                    {
                        "roles",rolesStrs
                        },
                });
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.OwinContext.Get<string>("as:client_id");
            //       var currentClient = context.OwinContext.Request.Get<string>("as:client_id");

            ////var currentClient = context.ClientId;
            //http://leastprivilege.com/2013/11/15/adding-refresh-tokens-to-a-web-api-v2-authorization-server/
            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var renwewRefreshToken = newIdentity.Claims.Where(c => c.Type == "RenwewRefreshToken").FirstOrDefault();
            if (renwewRefreshToken != null)
                newIdentity.RemoveClaim(renwewRefreshToken);

            newIdentity.AddClaim(new Claim("RenwewRefreshToken", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")));


            //  var currentClient = context.OwinContext.Get<string>("m:expiredOn");
            if (context.Ticket.Properties.Dictionary != null && context.Ticket.Properties.Dictionary.ContainsKey("m:expiredOn"))
            {
                //refresh custom expire date
                var expiredOn = DateTime.UtcNow.AddMinutes(General.MAXMinutesExpiredApiToken);
                context.Ticket.Properties.Dictionary["m:expiredOn"] = expiredOn.ToString("yyyy-MM-dd HH:mm:ss");
            }


            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
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