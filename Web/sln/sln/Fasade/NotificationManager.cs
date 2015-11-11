﻿using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Fasade
{
    public class NotificationManager
    {
        public NotificationManager()
        {

        }

        public async Task Send(ApplicationDbContext context, UserContext user, NotifyItem notifyItem)
        {
            var userDevices = context.UserNotify.Where(u => u.UserId == user.UserId && u.IsActive == true).ToList();
            var dt = DateTime.Now;
            context.NotifyMessage.Add(
                new DataModel.NotifyMessage
                {
                    Body = notifyItem.Body,
                    CreatedBy = user.UserId,
                    CreatedOn = dt,
                    IsActive = true,
                    IsRead = false,
                    ModifiedBy = user.UserId,
                    ModifiedOn = dt,
                    NotifyMessageId = Guid.NewGuid(),
                    Title = notifyItem.Title,
                    ToUrl = notifyItem.Url,
                    UserId = user.UserId
                });
            await context.SaveChangesAsync();

            foreach (var userDevice in userDevices)
            {
                bool isOk = SendPushServer(userDevice.DeviceId);
                if (!isOk)
                {
                    userDevice.IsActive = false;
                    userDevice.ModifiedBy = user.UserId;
                    userDevice.ModifiedOn = dt;
                    context.Entry<UserNotify>(userDevice).State = System.Data.Entity.EntityState.Modified;
                }
                await context.SaveChangesAsync();
            }
        }

        bool SendPushServer(string deviceid)
        {
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["NotificationServer"].ToString();// +"?d=" + deviceid;
               // url = "https://imaot.co.il/t/SendNotify.ashx";
                WebRequest tRequest;
                //http://5.100.251.87:4545/Test/m.html
                tRequest = WebRequest.Create(new Uri(url));
                tRequest.Method = "Post";
                tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                string postData = "d=" + deviceid + "";


                Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;


                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();

                if (sResponseFromServer.Contains("Error"))
                    return false;
                return true;
            }
            catch (Exception e)
            {

                return false;
            }

        }
    }
}