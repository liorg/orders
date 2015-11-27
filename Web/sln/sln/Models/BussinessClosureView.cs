using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class BussinessClosureItem
    {
        [Display(Name = "מזהה")]
        public Guid SlaId { get; set; }
        [Display(Name = "שם")]
        public string Name { get; set; }
        [Display(Name = "שעת פתיחה")]
        public string Start { get; set; }
        [Display(Name = "שעת סגירה")]
        public string End { get; set; }
        [Display(Name = "תאריך ספציפי")]
        public string DateSpiceial { get; set; }
        [Display(Name = "חופשה")]
        public bool  IsDateOff { get; set; }
    }
    public class BussinessClosureView : IComapnyView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        List<BussinessClosureItem> Items { get; set; }

    }
}