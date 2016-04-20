using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Helper;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;


namespace Michal.Project.Mechanism.Sync.Base
{
    public abstract class PushAdaptor : SyncAdaptorBase
    {

        public PushAdaptor(ApplicationDbContext context)
            : base(context)
        {


        }

        public abstract ISyncObject GetConfig();

        public abstract Dictionary<Guid, int> AdditionUsers();

        public virtual int SyncDirection
        {
            get
            {
                return SyncStatus.SyncFromServer;
            }
        }

        public virtual string ClientId
        {
            get
            {
                return General.NgAutoApp;
            }
        }

        public virtual async Task Push()
        {
            var logic = GetLogic(_context);
            var config = GetConfig();
            var users = new Dictionary<Guid, int>();

            users = AdditionUsers();

            foreach (var user in users)
            {
                var devices = await logic.GetDevicesByUserId(user.Key);
                foreach (var deviceUser in devices)
                {
                    ItemSync itemSync = new ItemSync(config);
                    itemSync.DeviceId = deviceUser.DeviceId;
                    itemSync.UserId = deviceUser.UserId;
                    itemSync.ClientId = deviceUser.ClientId;
                    itemSync.SyncStateRecord = user.Value;// SyncStateRecord.Change;
                    itemSync.SyncStatus = SyncDirection;
                    itemSync.ClientId = ClientId;
                    itemSync.LastUpdateRecord = DateTime.UtcNow;
                    await logic.SyncOn(itemSync);
                    await _context.SaveChangesAsync();
                    await SendPushServerAsync(itemSync.DeviceId, "data has changed", itemSync.ObjectId.ToString(), NotifyItemMessage.MESSAGE_CHANGEDATA);
                }
            }
        }

        public virtual async Task SyncAll()
        {
            var users = AdditionUsers();
            var config = GetConfig();
            var logic = GetLogic(_context);
            foreach (var user in users)
            {
                var devices = await logic.GetDevicesByUserId(user.Key);
                foreach (var deviceUser in devices)
                {
                    ItemSync itemSync = new ItemSync(config);
                    itemSync.DeviceId = deviceUser.DeviceId;
                    itemSync.UserId = deviceUser.UserId;
                    itemSync.ClientId = deviceUser.ClientId;
                    itemSync.LastUpdateRecord = DateTime.UtcNow;
                    await logic.DeleteSyncFlags(itemSync);
                    await _context.SaveChangesAsync();
                }
            }

        }

        protected virtual async Task<bool> SendPushServerAsync(string deviceid, string body, string recid, string messageType)
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