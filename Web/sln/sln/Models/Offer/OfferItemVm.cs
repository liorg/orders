using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferItemVm
    {
        public Guid Id { get; set; }
        [Display(Name = "פרטים נוספים")]
        public string Text { get; set; }

        [Display(Name = "שם החברה")]
        public Guid CompanyId { get; set; }

        [Display(Name = "שם הלקוח")]
        public Guid OrgId { get; set; }

        [Display(Name = "מחיר")]
        public double  Price { get; set; }

        public int Status { get; set; }
    }
}