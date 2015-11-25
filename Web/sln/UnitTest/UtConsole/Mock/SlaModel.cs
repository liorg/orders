using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtConsole.Mock
{
    public class SlaModel : ISla
    {
        public Guid SlaId { get; set; }

        public Guid? Organizations_OrgId { get; set; }
        public Guid? Distance_DistanceId { get; set; }
        public Guid? ShipType_ShipTypeId { get; set; }
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }

        public double Mins { get; set; }
    }
}
