using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class Sla : IModifieder
    {
        public Sla()
        {

        }
        [Required]
        public int Priority { get; set; }

        [Required]
        public bool IsBusinessDay { get; set; }

        [Required]
        public double Days { get; set; }

        [Required]
        public double Hours { get; set; }

        [Required]
        public double Mins { get; set; }


        public virtual ShipType ShipType { get; set; }
        [ForeignKey("ShipType")]
        public Guid? ShipType_ShipTypeId { get; set; }

        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }

        public virtual Distance Distance { get; set; }
        [ForeignKey("Distance")]
        public Guid? Distance_DistanceId { get; set; }

        public virtual Organization Organizations { get; set; }
        [ForeignKey("Organizations")]
        public Guid? Organizations_OrgId { get; set; }

        [Key]
        public Guid SlaId { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedOn
        {
            get;
            set;
        }

        public DateTime? ModifiedOn
        {
            get;
            set;
        }

        public Guid? CreatedBy
        {
            get;
            set;
        }

        public Guid? ModifiedBy
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }
    }
}