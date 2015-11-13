﻿
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
            var dt = DateTime.Now;
            result.ErrDesc = "ok";
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    context.UserNotify.Add(new DataModel.UserNotify
                    {
                        UserNotifyId = Guid.NewGuid(),
                        CreatedOn = dt,
                        DeviceId = deviceid,
                        IsActive = true,
                        ModifiedOn = dt,
                        UserId = Guid.Parse(userid)
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
        public HttpResponseMessage GetNotify(string deviceid)
        {
            Result<Notify> result = new Result<Notify>();
            var dt = DateTime.Now;
            result.ErrDesc = "ok";
            var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
            var path= "/Notification/";
            using (var context = new ApplicationDbContext())
            {
                result.Model = new Notify();
                result.Model.Items = new List<NotifyItem>();
                result.Model.Url = url+path;
                result.Model.Title = "מערכת הודעות זמן אמת";
                try
                {
                    var user = context.UserNotify.Where(d => d.DeviceId == deviceid).Select(s => s.UserId).FirstOrDefault();
                    if (user != null && user != Guid.Empty)
                    {

                        result.Model.Body = "שים לב יש לך הודעות חדשות";
                        var notifyMessages = context.NotifyMessage.Where(u => u.UserId == user && u.IsActive == true && u.IsRead == false).ToList();
                        foreach (var notifyMessage in notifyMessages)
                        {
                            var notifyItem = new NotifyItem
                            {
                                Body = notifyMessage.Body,
                                Title = result.Model.Title,
                                Url = notifyMessage.ToUrl,
                                Id = notifyMessage.NotifyMessageId
                            };
                            result.Model.Items.Add(notifyItem);
                            notifyMessage.IsRead = true;
                            context.Entry<NotifyMessage>(notifyMessage).State = EntityState.Deleted;
                        }
                        context.SaveChanges();
                    }
                    else
                    {

                        result.Model.Body = "אן לך הודעות חדשות";

                    }

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

    }
}