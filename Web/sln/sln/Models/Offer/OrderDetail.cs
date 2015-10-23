using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderDetail : IShipView
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "כתובת יעד")]
        public AddressEditorViewModel TargetAddress { get; set; }

        [Display(Name = "כתובת מקור")]
        public AddressEditorViewModel SourceAddress { get; set; }

    }
}