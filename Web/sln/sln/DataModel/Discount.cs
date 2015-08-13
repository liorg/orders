﻿using Michal.Project.Contract;
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

        }

        PriceList PriceList { get; set; }
        public Guid? PriceList_PriceListId { get; set; }

        [Key]
        public Guid DiscountId { get; set; }

       // public decimal Present { get; set; }

        public virtual Distance Distance { get; set; }
        [ForeignKey("Distance")]
        public Guid? Distance_DistanceId { get; set; }

        //public int? MinQuantityOneWay { get; set; }

       // public decimal? MinTimeWait { get; set; }

        public int QuantityType { get; set; }
        public int? MinQuantity{ get; set; }
        public int? MaxQuantity { get; set; }

        public decimal PriceValue { get; set; }// by the type of PriceValueType
        public int PriceValueType { get; set; } // 1= precent,2=fixed price,3=Credit(זיכוי)

        

        //[Column(TypeName = "Money")]
        //public decimal? DecreasePriceFixed { get; set; } // when is populate decrease price

        public string Name { get; set; }

        //public decimal Precent { get; set; }

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