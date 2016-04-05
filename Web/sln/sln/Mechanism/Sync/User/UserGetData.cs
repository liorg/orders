using Michal.Project.Dal;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Mechanism.Sync.User
{
    public class UserChange : PollAdaptor<ItemSync<WhoAmI>>
    {
        public UserChange(ApplicationDbContext context)
            : base(context)
        {

        }
       
        public override System.Threading.Tasks.Task<IEnumerable<ItemSync<WhoAmI>>> Poll(Contract.View.ISync request)
        {
            throw new NotImplementedException();
        }

        public override async System.Threading.Tasks.Task<ItemSync<WhoAmI>> PollItem(Contract.View.ISyncItem request)
        {
            var logic = GetLogic(_context);
            return await logic.GetMyDetail(request);
        }
    }
}