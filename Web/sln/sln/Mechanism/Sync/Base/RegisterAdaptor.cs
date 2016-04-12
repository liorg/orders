using Michal.Project.Contract.View;
using Michal.Project.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.Base
{
    public abstract class RegisterAdaptor : SyncAdaptorBase 
    {
        public RegisterAdaptor(ApplicationDbContext context)
            : base(context)
        {


        }

        protected  abstract Task<IEnumerable<ISyncItem>> GetItems (ISync syncDetail);

        public virtual async Task NotifyDataToUser(ISync syncDetail)
        {
          var items= await GetItems(syncDetail);
          foreach (var item in items)
          {
              
          }
        }
    }
}