
using Michal.Project.Bll;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
    [RoutePrefix("api/Notifcation")]
    public class NotifcationServiceController : ApiController
    {
        [Route("Register")]
        [AcceptVerbs("GET")]
        public async Task<HttpResponseMessage> Register(string userid, string deviceid)
        {
            Result result = new Result();
            var dt = DateTime.Now;
            result.ErrDesc = "ok";
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var notifyRepo = new NotificationRepository(context);
                    var shipRepo = new ShippingRepository(context);
                    var logic = new NotifyLogic(notifyRepo, shipRepo);
                    await logic.Register(userid, deviceid);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    result.IsError = true;
                    result.ErrDesc = e.ToString();
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<Result>(result,
                               new JsonMediaTypeFormatter(),
                                new MediaTypeWithQualityHeaderValue("application/json"))
                };
                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoStore = true;
                return response;
            }
        }

        [Route("GetNotify")]
        [AcceptVerbs("GET")]
        public async Task<HttpResponseMessage> GetNotify(string deviceid)
        {
            Result<NotifyItem> result = new Result<NotifyItem>();
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var notifyRepo = new NotificationRepository(context);
                    var shipRepo = new ShippingRepository(context);
                    var logic = new NotifyLogic(notifyRepo, shipRepo);
                    result.Model = await logic.GetNotifyForCloudMessage(deviceid);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    result.IsError = true;
                    result.ErrDesc = e.ToString();
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                }

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<Result<NotifyItem>>(result,
                               new JsonMediaTypeFormatter(),
                                new MediaTypeWithQualityHeaderValue("application/json"))
                };
                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoStore = true;
                return response;
            }
        }

    }
}
