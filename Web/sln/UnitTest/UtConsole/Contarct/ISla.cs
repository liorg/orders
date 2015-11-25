using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface ISla
    {
        Guid SlaId { get; set; }

        Guid? Organizations_OrgId { get; set; }
        Guid? Distance_DistanceId { get; set; }
        Guid? ShipType_ShipTypeId { get; set; }
        Guid? ShippingCompany_ShippingCompanyId { get; set; }

        double Mins { get; set; }

    }
   
}
