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
    public class UserUpdateData:PushAdaptor
    {
        EditUserViewModel _request;
        ISyncObject _syncObject;
        public UserUpdateData(ApplicationDbContext context, EditUserViewModel request)
            : base(context)
        {
            _request = request;

            _syncObject = new SyncObject
            {
                ObjectTableCode = ObjectTableCode.USER,
                ObjectId = Guid.Parse(_request.UserId)
            };
                

        }
        public override Contract.View.ISyncObject GetConfig()
        {

            return _syncObject;
        }

        public override Dictionary<Guid, int> AdditionUsers()
        {
            Dictionary<Guid, int> users = new Dictionary<Guid, int>();
            var logic = GetLogic(_context);
            var notifyRunners = logic.GetRunners();
            foreach (var runner in notifyRunners)
            {
                users.Add(Guid.Parse(runner.Id), SyncStateRecord.Change);
            }
            return users;
        }
    }
}