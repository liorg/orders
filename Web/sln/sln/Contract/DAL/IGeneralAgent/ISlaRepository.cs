
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface ISlaRepository
    {
        double FindSlaOnMinute(Guid shipCopanyId, Guid orgid, Guid distanceId, Guid shipTypeId);

        IEnumerable<Sla> GetAllSla(Guid orgid,Guid companyid);
        
    }
}
