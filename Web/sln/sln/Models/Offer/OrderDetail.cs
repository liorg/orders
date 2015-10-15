using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderDetail
    {
        public List<ItemPrice> Items { get; set; }
        public OrderItemVm OrderItemVm { get; set; }
    }
}