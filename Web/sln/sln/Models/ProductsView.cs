using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ProductsViewItem
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }
        [Display(Name = "שם")]
        public string Name { get; set; }
        [Display(Name = "תיאור")]
        public string Desc { get; set; }
        [Display(Name = " ממרחק (בק''מ)")]
        public string FromDistance { get; set; }
        [Display(Name = " עד מרחק (בק''מ)")]
        public string ToDistance { get; set; }
    
    }
    public class ProductsView : IView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProductsViewItem> Items { get; set; }

    }
}