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
        Dictionary<Guid, int> _users;

        protected Dictionary<Guid, int> Users
        {
            get
            {
                return _users;
            }
        }
     
        public PushAdaptor(ApplicationDbContext context)
            : base(context)
        {
          
            _users = new Dictionary<Guid, int>();
            NotifyUsers(_users);

        }

        protected abstract void NotifyUsers(Dictionary<Guid, int> users);

        public virtual async Task Push(ISyncItem request)
        {
            var logic = GetLogic(_context);
            foreach (var user in _users)
            {
                await logic.SyncFlagOn(request);
                await _context.SaveChangesAsync();
                await SendPushServerAsync(request.DeviceId, "data has changed", request.ObjectId.ToString(), NotifyItemMessage.MESSAGE_CHANGEDATA);    
            }
        }

        public virtual async Task SyncAll(ISync request)
        {
            var logic = GetLogic(_context);
            await logic.DeleteSyncFlags(request);
            await _context.SaveChangesAsync();
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