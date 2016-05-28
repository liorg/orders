using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Helper;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Mechanism.Sync.User
{
    public class WhoAmIUpdateData:PushAdaptor
    {
         ItemSync<WhoAmI> _request;

         public override DateTime GetLastUpdate
         {
             get
             {
                 return _request.LastUpdateRecord;
             }
             set
             {
                 _request.LastUpdateRecord = value;
             }
         }

        public WhoAmIUpdateData(ApplicationDbContext context, ItemSync<WhoAmI> request)
            : base(context)
        {
            _request = request;
        }

        public override Contract.View.ISyncObject GetConfig()
        {
            return _request;
        }

        public override int SyncDirection
        {
            get
            {
                return _request.SyncStatus;
            }
        }

        public override Dictionary<Guid, int> AdditionUsers(UserContext currentUser)
        {
            Dictionary<Guid, int> users = new Dictionary<Guid, int>();
            var logic = GetLogic(_context);
            var notifyRunners = logic.GetRunners();
            foreach (var runner in notifyRunners)
              users.Add(Guid.Parse(runner.Id), SyncStateRecord.Change);
            
            return users;
        }
    }
}