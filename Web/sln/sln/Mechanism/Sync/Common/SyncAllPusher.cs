using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Helper;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.User
{
    public class SyncUserPusher:PushAdaptor
    {
        ISyncItem _request;


        public SyncUserPusher(ApplicationDbContext context, ISyncItem user)
            : base(context)
        {
            _request = user;
        }

        public override Contract.View.ISyncObject GetConfig()
        {
            return _request;
        }

        public override Dictionary<Guid, int> AdditionUsers()
        {
            return null;
        }
        public override async Task SyncAll()
        {
            var config = GetConfig();
            var logic = GetLogic(_context);

            ItemSync itemSync = new ItemSync(config);
            itemSync.DeviceId = "";
            itemSync.UserId = _request.UserId;
            itemSync.ClientId = "";
            itemSync.ObjectId = Guid.Empty;
            itemSync.ObjectTableCode = ObjectTableCode.NONE;

            await logic.DeleteSyncFlags(itemSync);
            await _context.SaveChangesAsync();
        }
    }
}