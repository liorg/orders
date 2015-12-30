using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Providers
{
    //http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (var dbcontext = new ApplicationDbContext())
            {
                var repo = new UserRepository(dbcontext);
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = HelperSecurity.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = HelperSecurity.GetHash(context.Token);

            //using (AuthRepository _repo = new AuthRepository())
            using (var dbcontext = new ApplicationDbContext())
            {
                var repo = new UserRepository(dbcontext);
                var refreshToken = await repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}