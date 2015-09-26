﻿using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class ShippingCompany : IModifieder
    {
        public ShippingCompany()
        {
            Organizations = new HashSet<Organization>();
            Shippings = new HashSet<Shipping>();
            Users = new HashSet<ApplicationUser>();
            PriceLists = new HashSet<PriceList>();
        }

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

    }
}