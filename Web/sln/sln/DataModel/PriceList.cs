using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class PriceList : IModifieder
    {
        public PriceList()
        {

        }
        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }

        public virtual Organization Organizations { get; set; }
        [ForeignKey("Organizations")]
        public Guid? Organizations_OrgId { get; set; }

        [Required]
        public Guid ObjectId { get; set; }

        [Required]
        public int ObjectTypeCode { get; set; } // 1 =product,2=productsystem,3=distance,4=shiptype

        [Key]
        public Guid PriceListId { get; set; }

        public decimal? PriceValue { get; set; } 

        //public decimal PriceType { get; set; } //1 =fixed,2=%present

        public int  PriceValueType { get; set; } //1 =fixed,2=%present

        public int QuntityType { get; set; } //0 =unit,1=min
        public string Name { get; set; }

        public string Desc { get; set; }

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