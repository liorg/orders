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
    public abstract class PollAdaptor : SyncAdaptorBase
    {

        public PollAdaptor(ApplicationDbContext context)
            : base(context)
        {

        }

        public abstract Task<IEnumerable<T>> Poll<T>(ISync request) where T : ISyncItem;

        public abstract Task<T> PollItem<T>(ISyncItem request) where T : ISyncItem;

    }
   
}