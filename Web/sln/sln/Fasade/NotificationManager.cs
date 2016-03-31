using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Fasade
{
    public class NotificationManager 
    {
        public NotificationManager()
        {

        }

        public async Task SendItemsAsync(ApplicationDbContext context, MessageForUsers messageForUsers)
        {
            foreach (var user in messageForUsers.Users)
            {
                await SendAsync(context, user, messageForUsers.NotifyItem);
            }
        }

        public async Task SendAsync(ApplicationDbContext context, Guid? user, NotifyItem notifyItem)
        {
            if (!user.HasValue)
                return;
            var userDevices = context.UserNotify.Where(u => u.UserId == user.Value && u.IsActive == true).ToList();
            var dt = DateTime.Now;
            var notifyMessage= new DataModel.NotifyMessage
                {
                    Body = notifyItem.Body,
                    CreatedBy = user.Value,
                    CreatedOn = dt,
                    IsActive = true,
                    IsRead = false,
                    ModifiedBy = user.Value,
                    ModifiedOn = dt,
                    NotifyMessageId = Guid.NewGuid(),
                    Title = notifyItem.Title,
                    ToUrl = notifyItem.Url,
                    UserId = user.Value
                };
            context.NotifyMessage.Add(notifyMessage);
            await context.SaveChangesAsync();

            foreach (var userDevice in userDevices)
            {
                bool isOk = await SendPushServerAsync(userDevice.DeviceId, notifyMessage.Body, notifyItem.RecID, notifyItem.TypeMessage); //SendPushServer(userDevice.DeviceId);
                if (!isOk)
                {
                    userDevice.IsActive = false;
                    userDevice.ModifiedBy = user.Value;
                    userDevice.ModifiedOn = dt;
                    context.Entry<UserNotify>(userDevice).State = System.Data.Entity.EntityState.Modified;
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> SendPushServerAsync(string deviceid, string body,string recid,string messageType)
        {
            try
            {
            //    body = "ok מה קורה";
                var url = System.Configuration.ConfigurationManager.AppSettings["NotificationServer"].ToString();
                var formContent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("d", deviceid), 
                            new KeyValuePair<string, string>("b", body) ,
                            new KeyValuePair<string, string>("recid", recid) ,
                            new KeyValuePair<string, string>("t", messageType) 
                        });

                using (var myHttpClient = new HttpClient())
                {
                    var response = await myHttpClient.PostAsync(url, formContent);
                    var stringContent = await response.Content.ReadAsStringAsync();


                    if (stringContent.Contains("Error"))
                        return false;
                    return true;
                }
               
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                return false;
            }

        }
    }
}

