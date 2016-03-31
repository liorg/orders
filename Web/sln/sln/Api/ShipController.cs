using Michal.Project.Agent;
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
        [Route("GetShipsSync")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> GetShipsSync(RequestSync request)
        {
            ResponseBase<IEnumerable<ItemSync< ShippingVm>>> result = new ResponseBase<IEnumerable<ItemSync< ShippingVm>>>();
            result.Model = new List<ItemSync<ShippingVm>>();
            try
            {
                if (User!=null && User.Identity!=null && User.Identity.IsAuthenticated)
                {
                    result.IsAuthenticated = true;
                    using (var context = new ApplicationDbContext())
                    {
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);

                        IOfferRepository offerRepository = new OfferRepository(context);
                        IShippingRepository shippingRepository = new ShippingRepository(context);
                        GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                        IUserRepository userRepository = new UserRepository(context);

                        ViewLogic view = new ViewLogic(shippingRepository, userRepository, generalRepo);
                        result.Model = await view.GetMyShipsAsync(user.UserId, request.DeviceId,request.ClientId);

                    }
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrDesc = e.ToString();
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<ResponseBase<IEnumerable<ItemSync<ShippingVm>>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        [Route("UpdateShipSync")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> UpdateShipSync(ItemSync<ShippingVm> request)
        {
            ResponseBase<ItemSync<ShippingVm>> result = new ResponseBase<ItemSync<ShippingVm>>();
            result.Model = new ItemSync<ShippingVm>();
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
                        ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());

                        OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                        result.Model = await logic.OnPostUpdateShipSync(request, user);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrDesc = e.ToString();
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<ResponseBase<ItemSync<ShippingVm>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }


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
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result<IEnumerable<ShippingVm>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        [Route("WhoAmI")]
       [AcceptVerbs("GET")]
        public  HttpResponseMessage WhoAmI()
        {
            ResponseBase<WhoAmI> result = new ResponseBase<WhoAmI>();
            result.Model = new WhoAmI();
            result.Model.FullName = "Anonymous";
            result.Model.UserName = "Anonymous";

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    result.IsAuthenticated = true;
                    using (var context = new ApplicationDbContext())
                    {
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);
                        result.Model.UserName = user.FullName;
                        result.Model.FullName = user.FullName;
                        result.Model.UserId = user.UserId.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrDesc = e.ToString();
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<ResponseBase<WhoAmI>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }
    }
}