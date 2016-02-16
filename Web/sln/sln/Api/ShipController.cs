using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Michal.Project.Api
{
    [RoutePrefix("api/Ship")]
    public class ShipController : ApiController
    {
        [Route("GetMyShips")]
        public async Task<HttpResponseMessage> GetMyShips()
        {
            Result<IEnumerable<ShippingVm>> result = new Result<IEnumerable<ShippingVm>>();
            result.Model = new List<ShippingVm>();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    using (var context = new ApplicationDbContext())
                    {
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);

                        IOfferRepository offerRepository = new OfferRepository(context);
                        IShippingRepository shippingRepository = new ShippingRepository(context);
                        GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                        IUserRepository userRepository = new UserRepository(context);

                        ViewLogic view = new ViewLogic(shippingRepository, userRepository, generalRepo);
                        result.Model = await view.GetMyShips(user.UserId);

                    }
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrDesc = e.ToString();
                //Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result<IEnumerable<ShippingVm>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }
    }
}