using Michal.Project.Agent;
using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.Fasade;
using Michal.Project.Helper;
using Michal.Project.Mechanism.Sync.User;
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
        public async Task<HttpResponseMessage> WhoAmI()
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
                        IShippingRepository shippingRepository = new ShippingRepository(context);
                        GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                        IUserRepository userRepository = new UserRepository(context);
                        ICommentRepository commentRepository = new CommentRepository(context);
                        ISyncRepository syncRepository = new SyncRepository(context);
                        INotificationRepository notificationRepository = new NotificationRepository(context);

                        var syncLogic = new SyncLogic(shippingRepository, userRepository, commentRepository, syncRepository, notificationRepository, generalRepo);
                        result.Model = await  syncLogic.GetWhoAmI(user.UserId);

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

        [Route("UpdateWhoAmI")]
        [AcceptVerbs("Post")]
        public async Task<HttpResponseMessage> UpdateWhoAmI([FromBody]WhoAmI request)
        {
            ResponseBase<WhoAmI> result = new ResponseBase<WhoAmI>();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    result.IsAuthenticated = true;
                    using (var context = new ApplicationDbContext())
                    {
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);
                        IUserRepository userRepository = new UserRepository(context);
                        UserLogic logic = new UserLogic(userRepository);

                        result.Model = await logic.UpadateQuick(request);
                        await context.SaveChangesAsync();
                         ItemSync<WhoAmI> sync = new ItemSync<Models.WhoAmI>();

                         sync.ObjectId = Guid.Parse(request.UserId);
                         sync.ObjectTableCode = ObjectTableCode.USER;
                         sync.SyncStateRecord = SyncStateRecord.Change;
                         sync.SyncStatus = SyncStatus.SyncFromClient;
                         sync.SyncObject = result.Model;
                         sync.LastUpdateRecord = DateTime.Now;


                         SyncManager syncManager = new SyncManager(user);
                        await syncManager.Push(new WhoAmIUpdateData(context, sync));

                    }
                }
                else
                {
                    result.IsAuthenticated = false;
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


        [Route("GetWhoAmISync")]
        [AcceptVerbs("GET")]
        public async Task<HttpResponseMessage> GetWhoAmISync([FromUri] Sync request)
        {
            ResponseBase<ItemSync<WhoAmI>> result = new ResponseBase<ItemSync<WhoAmI>>();
            result.Model = new ItemSync<Models.WhoAmI>();
            result.Model.SyncObject = new Models.WhoAmI
            {
                FullName = "Anonymous",
                UserName = "Anonymous"
            };
          

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    ItemSync<Guid> userContextSync = new ItemSync<Guid>();
                    userContextSync.ClientId = request.ClientId;
                    userContextSync.DeviceId = request.DeviceId;
                    userContextSync.UserId = request.UserId;
                    userContextSync.ObjectId = request.UserId;
                    
                    result.IsAuthenticated = true;
                    using (var context = new ApplicationDbContext())
                    {
                        // for test
                        
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);
                        userContextSync.ObjectTableCode = ObjectTableCode.USER;
                        SyncManager syncManager = new SyncManager(user);
                        var pollItem = await syncManager.pull(userContextSync, new UserGetData(context));

                        result.Model.ClientId = pollItem.ClientId;
                        result.Model.UserId = pollItem.UserId;
                        result.Model.DeviceId = pollItem.DeviceId;
                        result.Model.LastUpdateRecord = pollItem.LastUpdateRecord;
                        result.Model.ObjectId = pollItem.ObjectId;
                        result.Model.ObjectTableCode = pollItem.ObjectTableCode;
                        result.Model.SyncObject = pollItem.SyncObject;
                        result.Model.SyncStateRecord = pollItem.SyncStateRecord;
                        result.Model.SyncStatus = pollItem.SyncStatus;

                        var test = context.TableTest.FirstOrDefault();
                        if (test != null)
                        {
                            if (test.Code == 1)
                                result.IsAuthenticated = false;
                            if (test.Code == 2)
                                result.IsError = true;
                       


                        }
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
                Content = new ObjectContent<ResponseBase<ItemSync<WhoAmI>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }
         
        [Route("UpdateWhoAmISync")]
        [AcceptVerbs("Post")]
        public async Task<HttpResponseMessage> UpdateWhoAmISync([FromBody] ItemSync<WhoAmI> request)
        {
            ResponseBase<ItemSync<WhoAmI>> result = new ResponseBase<ItemSync<WhoAmI>>();
            result.Model = new ItemSync<WhoAmI>();
            try
            {
                //throw new  ArgumentNullException("erro on "+request.LastUpdateRecord.ToString("yyyy-MM-dd HH:mm"));
                if (User.Identity.IsAuthenticated)
                {
                    result.IsAuthenticated = true;
                    using (var context = new ApplicationDbContext())
                    {
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);
                        IUserRepository userRepository = new UserRepository(context);
                        UserLogic logic = new UserLogic(userRepository);

                        result.Model.SyncObject = await logic.UpdateSync(request);
                        await context.SaveChangesAsync();

                        SyncManager syncManager = new SyncManager(user);
                        await syncManager.Push(new WhoAmIUpdateData(context, result.Model));

                    }
                }
                else
                {
                    result.IsAuthenticated = false;
                }
                result.Model.LastUpdateRecord = request.LastUpdateRecord;
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.ErrDesc = e.ToString();
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<ResponseBase<ItemSync<WhoAmI>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        [Route("SyncAllWhoAmI")]
        [AcceptVerbs("Get")]
        public async Task<HttpResponseMessage> SyncWhoAmI([FromBody] ItemSync<WhoAmI> request)
        {
            ResponseBase<ItemSync<WhoAmI>> result = new ResponseBase<ItemSync<WhoAmI>>();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    result.IsAuthenticated = true;
                    using (var context = new ApplicationDbContext())
                    {
                        var userContext = HttpContext.Current.GetOwinContext().Authentication;
                        var user = new UserContext(userContext);
                        result.Model = request;
                        SyncManager syncManager = new SyncManager(user);
                        await syncManager.Sync(new SyncUserPusher(context, request));

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
                Content = new ObjectContent<ResponseBase<ItemSync<WhoAmI>>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        public StatusLogic GetLogin(ApplicationDbContext context)
        {
            IShippingRepository shippingRepository = new ShippingRepository(context);
            return new StatusLogic(shippingRepository);
        }

        //[Route("UpdateShipSync")]
        //[AcceptVerbs("POST")]
        //public async Task<HttpResponseMessage> UpdateShipSync(ItemSync<MobileShipStatusVm> request)
        //{
        //    ResponseBase<ItemSync<MobileShipVm>> result = new ResponseBase<ItemSync<MobileShipVm>>();
        //    result.Model = new ItemSync<MobileShipVm>();
        //    try
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            using (var context = new ApplicationDbContext())
        //            {
        //                var userContext = HttpContext.Current.GetOwinContext().Authentication;
        //                var user = new UserContext(userContext);

        //                IOfferRepository offerRepository = new OfferRepository(context);
        //                IShippingRepository shippingRepository = new ShippingRepository(context);
        //                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
        //                IUserRepository userRepository = new UserRepository(context);
        //                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());

        //                StatusLogic logic = new StatusLogic(shippingRepository);

        //                result.Model = await logic.ChangeStatusSync(request, user.UserId);
        //                await context.SaveChangesAsync();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        result.IsError = true;
        //        result.ErrDesc = e.ToString();
        //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        //    }
        //    var response = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ObjectContent<ResponseBase<ItemSync<MobileShipVm>>>(result,
        //                   new JsonMediaTypeFormatter(),
        //                    new MediaTypeWithQualityHeaderValue("application/json"))
        //    };
        //    return response;
        //}

    }
}