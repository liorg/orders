using Michal.Project.Contract.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtConsole.Mock
{
    public class MockSla : ISlaRepository
    {
        List<SlaModel> _data;
        public MockSla()
        {
            _data = new List<SlaModel>();
            _data.Add(new SlaModel
            {
                Distance_DistanceId = Guid.Parse("{91F046EA-0000-0000-0000-2FE8C59B235F}"),
                Organizations_OrgId = Guid.Parse("{91F046EA-0000-0000-0001-2FE8C59B235F}"),
                ShippingCompany_ShippingCompanyId = Guid.Parse("{91F046EA-0000-0000-0002-2FE8C59B235F}"),
                ShipType_ShipTypeId = Guid.Parse("{91F046EA-0000-0000-0003-2FE8C59B235F}"),
                Mins = 120
            });
        }
        public double FindSlaOnMinute(Guid shipCopanyId, Guid orgid, Guid distanceId, Guid shipTypeId)
        {
            var result=_data.Where(sl=>sl.Organizations_OrgId.HasValue && sl.Organizations_OrgId.Value==orgid &&
                                 sl.Distance_DistanceId.HasValue && sl.Distance_DistanceId.Value==distanceId   &&    
                                sl.ShippingCompany_ShippingCompanyId.HasValue && sl.ShippingCompany_ShippingCompanyId.Value==shipCopanyId   &&     
                                   sl.ShipType_ShipTypeId.HasValue && sl.ShipType_ShipTypeId.Value==shipTypeId   ).Select(m=>m.Mins).FirstOrDefault();
            return result;
        }
    }
}
