using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferDetail
    {
        public List<ItemPrice> Items{ get; set; }
        public List<DiscountItem> DiscountItems { get; set; }
        
        public OfferItemVm OfferItemVm { get; set; }
    }
}