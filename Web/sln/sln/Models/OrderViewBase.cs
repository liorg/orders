using Michal.Project.Contract;
using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderViewBase
    {
        public ShippingVm ShippingVm { get; set; }
        public Location Location { get; set; }
    }

    public class RunnerView : OrderViewBase, IView
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        public List<Runner> Runners { get; set; }
        public UserDetail CurrentRunner { get; set; }
    }

    public class RunnerDetail : OrderViewBase, IView
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        public IEnumerable<ShippingVm> Items { get; set; }
    }
}