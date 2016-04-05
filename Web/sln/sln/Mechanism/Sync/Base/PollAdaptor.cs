using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.Base
{
    public abstract class PollAdaptor<T> : SyncAdaptorBase  where T : ISyncItem
    {

        public PollAdaptor(ApplicationDbContext context)
            : base(context)
        {

        }

        public abstract Task<IEnumerable<T>> Poll(ISync request) ;

        public abstract Task<T> PollItem(ISyncItem request);

    }
   
}