using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Mechanism.Sync.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.User
{
    public class UserRegisterNotify : RegisterAdaptor
    {
        public UserRegisterNotify(ApplicationDbContext context)
            : base(context)
        {

        }
        protected override async Task<IEnumerable<ISyncItem>> GetItems(Contract.View.ISync syncDetail)
        {
            List<ISyncItem> syncItems = new List<ISyncItem>();
            var logic = GetLogic(_context);
            var data = await logic.GetMyShipsAsync(syncDetail.UserId, syncDetail.DeviceId, syncDetail.ClientId);
            var user = await logic.GetMyDetail(syncDetail);
            syncItems.AddRange(data);
            syncItems.Add(user);
           // ((IList)data).Add(user);
           /// data.ToList().Add(user);
            return data;
        }
    }
}