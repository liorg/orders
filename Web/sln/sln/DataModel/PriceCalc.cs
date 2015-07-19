﻿using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class PriceCalc : IModifieder
    {
        public PriceCalc()
        {

        }
        public int Order { get; set; }

        public Guid ShippingId { get; set; }

        public Guid ShippingItemId { get; set; }

        public Guid PriceListId { get; set; }

        public Guid DiscountId { get; set; }

        [Key]
        public Guid PriceCalcId { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        public decimal? Present { get; set; }

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