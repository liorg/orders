using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class SlaItem
    {
        [Display(Name = "מזהה")]
        public Guid SlaId { get; set; }
        //[Display(Name = "שם")]
        //public string Name { get; set; }
        [Display(Name = "מרחק")]
        public string Distance { get; set; }
        [Display(Name = "סוג משלוח")]
        public string ShipType { get; set; }
        [Display(Name = "תיאור")]
        public string Desc { get; set; }
        [Display(Name = "זמן תגובה בדקות")]
        public string Mins { get; set; }
    }
    public class SlaView : IView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SlaItem> Items { get; set; }

    }
}