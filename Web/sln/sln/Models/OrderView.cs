using Michal.Project.Contract;
using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderView : OrderViewStatus,IView
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }
        //public Location Location { get; set; }
        //public  ShippingVm  ShippingVm { get; set; }
        public List<ShippingItemVm> ShippingItems { get; set; }
        public IEnumerable<TimeLineVm> TimeLineVms { get; set; }
        //    public StatusVm Status { get; set; }
        public bool IsEyeOnHim { get; set; }
        public IEnumerable<CommentVm> CommentsVm { get; set; }
        public UserContext JobTitle { get; set; }
    }

    public class OrderViewStatus : OrderViewBase
    {

        public StatusVm Status { get; set; }
    }

}