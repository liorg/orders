using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class ShippingCompanyDecorator
    {
        public string ManagerFullName { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }
        public Guid ShippingCompanyId { get; set; }

        public Guid? ManagerId { get; set; }

        public Address AddressCompany { get; set; }

        public string Tel { get; set; }

        public string ContactFullName { get; set; }

        public string ContactTel { get; set; }


        public ShippingCompanyDecorator()
        {

        }

        public List<ApplicationUser> Users { get; set; }


    }
    public class ShippingCompany : IModifieder
    {
        public ShippingCompany()
        {
            Organizations = new HashSet<Organization>();
            Shippings = new HashSet<Shipping>();
            Users = new HashSet<ApplicationUser>();
            PriceLists = new HashSet<PriceList>();
            RequestShipping = new HashSet<RequestShipping>();
        }
        public ICollection<RequestShipping> RequestShipping { get; set; }

        public ICollection<PriceList> PriceLists { get; set; }
        // many to many
        public ICollection<Organization> Organizations { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<Shipping> Shippings { get; set; }

        [Key]
        public Guid ShippingCompanyId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

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

        public Guid? ManagerId { get; set; }

        public Address AddressCompany { get; set; }

        public string Tel { get; set; }

        public string ContactFullName { get; set; }

        public string ContactTel { get; set; }

        public string Perfix { get; set; } // for @ to detect username like d@ran d is user and ran is the comapny
    }
}