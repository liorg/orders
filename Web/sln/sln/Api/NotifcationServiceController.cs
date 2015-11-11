using Michal.Project.Dal;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Michal.Project.Api
{
    [RoutePrefix("api/Notifcation")]
    public class NotifcationServiceController : ApiController
    {
        [Route("Register")]
        [AcceptVerbs("GET")]
        public HttpResponseMessage Register(string userid, string deviceid)
        {
            Result result = new Result();
            var dt=DateTime.Now;
            result.ErrDesc = "ok";
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    context.UserNotify.Add(new DataModel.UserNotify
                    {
                        UserNotifyId=Guid.NewGuid(),
                        CreatedOn = dt,
                        DeviceId = deviceid,
                        IsActive = true,
                        ModifiedOn = dt,
                        UserId =Guid.Parse( userid)
                    });
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    result.IsError = true;
                    result.ErrDesc = e.ToString();
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
        public HttpResponseMessage GetNotify(Guid deviceid)
        {
            Result<Notify> result = new Result<Notify>();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result<Notify>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            response.Headers.CacheControl = new CacheControlHeaderValue();
            response.Headers.CacheControl.NoStore = true;
            return response;
        }

    }
}