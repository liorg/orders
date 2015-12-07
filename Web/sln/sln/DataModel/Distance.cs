﻿using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class Distance : IModifieder
    {
        public Distance()
        {
           Organizations = new HashSet<Organization>();
           Shippings = new HashSet<Shipping>();
           //PriceLists = new HashSet<PriceList>();
         //  Discounts = new HashSet<Discount>();
        }

        // many to many
        public ICollection<Organization> Organizations { get; set; }

      //  public ICollection<PriceList> PriceLists { get; set; }

        public ICollection<Shipping> Shippings { get; set; }

       // public ICollection<Discount> Discounts { get; set; }

        [Key]
        public Guid DistanceId { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }


        public float? FromDistance { get; set; }
        public float? ToDistance { get; set; }

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