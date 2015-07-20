﻿using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class Shipping : IModifieder
    {
        public Shipping()
        {
            ShippingItems = new HashSet<ShippingItem>();
            TimeLines = new HashSet<TimeLine>();
        }
        public ICollection<TimeLine> TimeLines { get; set; }
        public ICollection<ShippingItem> ShippingItems { get; set; }

        [Key]
        public Guid ShippingId { get; set; }

        public Distance Distance { get; set; }

        public StatusShipping StatusShipping { get; set; }

        public Organization Organization { get; set; }

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

        public City CityFrom { get; set; }
        public City CityTo { get; set; }

        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }

        public string AddressNumFrom { get; set; }
        public string AddressNumTo { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        public decimal TimeWait { get; set; }

        [Column(TypeName = "Money")]
        public decimal EstimatedPrice { get; set; }

    }
}