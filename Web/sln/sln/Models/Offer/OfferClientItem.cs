using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{

    public class OfferClientItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
       // public Guid StatusShipping { get; set; }

        public decimal? PriceValue
        {
            get
            {
                return ProductPrice.HasValue ? ProductPrice.Value * Amount : ProductPrice;
            }
        }
        public string QuntityType { get; set; }

        public bool IsPresent { get; set; }
        public bool IsDiscount { get; set; }
        public int StatusRecord { get; set; } // 1 =addbysystem,2=remove,3=addnew,4=edit
        public int Amount { get; set; }
        public decimal? ProductPrice { get; set; }

        public bool HasPrice
        {
            get
            {
                return PriceValue.HasValue;
            }
        }
        public bool AllowEdit
        {
            get;
            set;
        }
        public bool AllowRemove
        {
            get;
            set;
        }
        public bool AllowCancel
        {
            get;
            set;
        }
    }

}