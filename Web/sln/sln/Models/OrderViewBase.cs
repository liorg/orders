using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderViewBase
    {
        public ShippingVm ShippingVm { get; set; }
        public Location Location { get; set; }
    }
}