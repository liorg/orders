﻿using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class Organization : IModifieder
    {
        public Organization()
        {
            Users = new HashSet<ApplicationUser>();
            Distances = new HashSet<Distance>();
            PriceLists = new HashSet<PriceList>();
            Shippings = new HashSet<Shipping>();
            AddressOrg = new Address();
            ShippingCompany = new HashSet<ShippingCompany>();
            RequestShipping = new HashSet<RequestShipping>();
        }
        public ICollection<RequestShipping> RequestShipping { get; set; }
        public Address AddressOrg { get; set; }
        // many to many
        public ICollection<Distance> Distances { get; set; }
        // many to many
        public ICollection<ShippingCompany> ShippingCompany { get; set; }

        [ForeignKey("Organization_OrgId")]
        public ICollection<ApplicationUser> Users { get; set; }

        // many to many
        public ICollection<Product> Products { get; set; }

        public ICollection<PriceList> PriceLists { get; set; }


        // [ForeignKey("Organization_OrgId")]
        public ICollection<Shipping> Shippings { get; set; }

        [Key]
        public Guid OrgId { get; set; }

        public string Name { get; set; }

        public string Perfix { get; set; } // for autonumber

        public string Domain { get; set; }

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

        public decimal? PriceValueException { get; set; }

        // null (default) 1 = private (like bb),2=public(whatsup,public,facebook,gmail)
        public int? OrganizationCode { get; set; }
      
    }
}