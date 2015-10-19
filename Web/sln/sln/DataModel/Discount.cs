using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class Discount : IModifieder
    {
        public Discount()
        {
          //  RequestShipping = new HashSet<RequestShipping>();
        }
        // many to many
        //public ICollection<RequestShipping> RequestShipping { get; set; }
        public DateTime BeginDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }
        public virtual Organization Organizations { get; set; }
        [ForeignKey("Organizations")]
        public Guid? Organizations_OrgId { get; set; }

        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }
        [Key]
        public Guid DiscountId { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }

        [Required]
        public bool IsSweeping { get; set; }

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