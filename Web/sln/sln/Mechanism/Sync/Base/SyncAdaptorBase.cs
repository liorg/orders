using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.Base
{
    public abstract class SyncAdaptorBase
    {
        public SyncAdaptorBase(ApplicationDbContext context)
        {

        }
    }

}