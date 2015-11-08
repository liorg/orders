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
        [Display(Name = "מזהה משלוח")]
        public Guid Id { get; set; }
        [Display(Name = "מזהה הצעה")]
        public Guid OfferId { get; set; }

        [Display(Name = "מספר משלוח")]
        public string Name { get; set; }

        [Display(Name = " כותרת")]
        public string Title { get; set; }

        [Display(Name = "כתובת יעד")]
        public AddressEditorViewModel TargetAddress { get; set; }

        [Display(Name = "כתובת מקור")]
        public AddressEditorViewModel SourceAddress { get; set; }

    }
}