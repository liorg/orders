using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.User
{
    public class UserGetData : PollAdaptor<ItemSync<WhoAmI>>
    {
        public UserGetData(ApplicationDbContext context)
            : base(context)
        {

        }
       
        public override Task<IEnumerable<ItemSync<WhoAmI>>> Poll(ISync request)
        {
            throw new NotImplementedException();
        }

        public override async Task<ItemSync<WhoAmI>> PollItem(ISyncItem request)
        {
            var logic = GetLogic(_context);
            return await logic.GetMyDetail(request);
        }
    }
}