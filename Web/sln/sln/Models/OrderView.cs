using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class OrderView
    {
        public  ShippingVm  ShippingVm { get; set; }
        public List<ShippingItemVm> ShippingItems { get; set; }
        public IEnumerable<TimeLineVm> TimeLineVms { get; set; }

    }
}