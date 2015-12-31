using Michal.Project.Dal;
using Michal.Project.Helper;
using Michal.Project.Models;
using Michal.Project.Models.NSStreet;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using Michal.Project.DataModel;
using Michal.Project.Fasade;
using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.Agent;


namespace Michal.Project.Api
{
    [Authorize]
    [RoutePrefix("api/OfferService")]
    public class OfferServiceController : ApiController
    {
       
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("GetOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> GetOffer(Guid shipid, Guid offerId, Guid shippingCompanyId)
        {
            using (var context = new ApplicationDbContext())
            {
                bool allowRemove = false;
                bool allowEdit = false;
                bool allowExcepPrice = false;

                var userContext = HttpContext.Current.GetOwinContext().Authentication;

                MemeryCacheDataService cache = new MemeryCacheDataService();
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                   (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
                {
                    allowRemove = true;
                    allowEdit = true;
                }
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                   (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.ApprovalExceptionalBudget) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
                {
                    allowExcepPrice = true;
                }
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                var user = new UserContext(userContext);
                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context,new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                var ship = await shippingRepository.GetShipIncludeItems(shipid);
                OfferClient offerClient = logic.GetOfferClient(allowRemove, allowEdit, ship, shippingCompanyId, user);
                offerClient.AddExceptionPrice = allowExcepPrice;
                if (offerId == Guid.Empty)
                {
                    logic.AppendNewOffer(offerClient, ship, allowRemove, allowEdit);
                }
                else
                {
                    var offer = await offerRepository.GetOfferAndHisChilds(offerId);
                    logic.AppendCurrentOffer(offerClient, ship, offer, allowRemove, allowEdit);
                }
                var data = JsonConvert.SerializeObject(offerClient);
                var responseBody = @"var offerClient = " + data + ";";

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/javascript")
                };

                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoStore = true;
                return response;
            }
        }
        
        [Authorize]
        [System.Web.Http.AcceptVerbs("POST")]
        [Route("CommitOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> CommitOffer([FromBody] OfferUpload offer)
        {
            var result = new Result<OfferMessage>();
            var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            using (var context = new ApplicationDbContext())
            {
                OfferManager offermanager = new OfferManager();
                var isUserGrant=this.User.IsInRole(HelperAutorize.RoleOrgManager) || this.User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.ApprovalExceptionalBudget);

                result = await offermanager.CommitAsync(context, offer, user, isUserGrant);

            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result<OfferMessage>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }
        
        [Authorize]
        [System.Web.Http.AcceptVerbs("POST")]
        [Route("CancelOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> CancelOffer([FromBody] OfferUpload offer)
        {
            var result = new Result();
            var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            using (var context = new ApplicationDbContext())
            {
                OfferManager offermanager = new OfferManager();
                result = await offermanager.CancelAsync(context, offer, user);

            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

         [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("GetMockOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> GetMockOffer(Guid shippingCompanyId)
        {
            using (var context = new ApplicationDbContext())
            {
                bool allowRemove = true;
                //    bool allowAdd = false;
                bool allowEdit = true;
                var userContext = HttpContext.Current.GetOwinContext().Authentication;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                var user = new UserContext(userContext);
                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository,locationRepository);
                 
                var mockShip = new Shipping();
                mockShip.Direction = 0;
                mockShip.ShippingItems = new List<ShippingItem>();
                
                DefaultShip defaultShip = new DefaultShip();

                var shtype = defaultShip.items.Where(t => t.Item1 == DefaultShip.DType.ShipType).FirstOrDefault();
                mockShip.ShipType = new ShipType();
                mockShip.ShipType.ShipTypeId = shtype.Item3;
                mockShip.ShipType.Name = shtype.Item2;

                var dist = defaultShip.items.Where(t => t.Item1 == DefaultShip.DType.Distance).FirstOrDefault();
                mockShip.Distance = new Distance();
                mockShip.Distance.DistanceId = dist.Item3;
                mockShip.Distance.Name = dist.Item2;

             
                OfferClient offerClient = logic.GetOfferClient(allowRemove, allowEdit, mockShip, shippingCompanyId, user);
                offerClient.IsDemo = true;
                logic.AppendNewOffer(offerClient, mockShip, allowRemove, allowEdit);

                var data = JsonConvert.SerializeObject(offerClient);
                var responseBody = @"var offerClient = " + data + ";";

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/javascript")
                };

                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoStore = true;
                return response;
            }
        }
    }
}
