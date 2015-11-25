using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface ISlaValue
    {
        DateTime? SlaTime { get; set; }
        DateTime? ActualStartDate { get; set; }

        Guid? Distance_DistanceId { get; set; }
        Guid? ShipType_ShipTypeId { get; set; }
        Guid? ShippingCompany_ShippingCompanyId { get; set; }
        Guid? Organization_OrgId { get; set; }
    }
}
