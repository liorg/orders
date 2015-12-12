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

        ShippingCompany _company;
        public ShippingCompany ShippingCompany
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }
        public Guid ShippingCompanyId
        {
            get
            {
                return _company.ShippingCompanyId;
            }
        }
        public string Name
        {
            get
            {
                return _company.Name;
            }
        }
        public ShippingCompanyDecorator()
        {
            _company = new ShippingCompany();
        }
        public ShippingCompanyDecorator(ShippingCompany comany)
        {
            _company = new ShippingCompany();
            _company.Tel = comany.Tel;
            _company.Users = comany.Users;
            _company.ShippingCompanyId = comany.ShippingCompanyId;
            _company.AddressCompany = comany.AddressCompany;
            _company.ContactFullName = comany.ContactFullName;
            _company.ContactTel = comany.ContactTel;
            _company.Desc = comany.Desc;
            _company.IsActive = comany.IsActive;
            _company.ManagerId = comany.ManagerId;
            _company.Name = comany.Name;
           
        }
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
    }
}