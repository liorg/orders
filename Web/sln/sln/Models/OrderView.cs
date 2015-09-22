﻿using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderView : OrderViewBase
    {
        //public Location Location { get; set; }
        //public  ShippingVm  ShippingVm { get; set; }
        public List<ShippingItemVm> ShippingItems { get; set; }
        public IEnumerable<TimeLineVm> TimeLineVms { get; set; }
        public StatusVm Status { get; set; }
        public bool IsEyeOnHim { get; set; }
        public IEnumerable<CommentVm> CommentsVm { get; set; }
        public UserContext JobTitle { get; set; }
    }

    

}