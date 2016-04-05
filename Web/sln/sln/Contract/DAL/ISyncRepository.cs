using Michal.Project.Contract.View;
using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface ISyncRepository
    {
        Task<IEnumerable<SyncTable>> GetSyn(Guid userId, Guid objectId, int objectTableCode);
        Task DeleteUnused(ISync sync);
        Task FlagOn(ISyncItem sync);
    
    }
}
