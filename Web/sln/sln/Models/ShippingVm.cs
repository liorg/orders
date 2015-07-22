using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class ShippingVm
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "סטאטוס")]
        public string Status { get; set; }

        [Display(Name = "כתובת מקור")]
        public string SreetFrom { get; set; }

        [Display(Name = "מספר כתובת מקור ")]
        public string NumFrom { get; set; }

        [Display(Name = "כתובת יעד ")]
        public string SreetTo{ get; set; }

        [Display(Name = "מספר כתובת יעד ")]
        public string NumTo { get; set; }

        [Display(Name = "סטאטוס")]
        public Guid StatusId { get; set; }

        [Display(Name = "עיר מקור")]
        public Guid CityForm { get; set; }

        [Display(Name = "עיר יעד")]
        public Guid CityTo { get; set; }

        [Display(Name = "חישוב היעד")]
        public Guid DistanceId { get; set; }

        [Display(Name = "תאריך יצירה ")]
        public string CreatedOn { get; set; }

        [Display(Name = "נוצר ע''י ")]
        public string CreatedBy { get; set; }

        [Display(Name = "חישוב היעד")]
        public string DistanceName { get; set; }

        [Display(Name = "עיר מקור")]
        public string CityFormName { get; set; }

        [Display(Name = "עיר יעד")]
        public string CityToName { get; set; }

        [Required]
        [Display(Name = "ארגון")]
        public Guid OrgId { get; set; }

        [Display(Name = "מספר  הזמנה ")]
        public string Number { get; set; }


        [Display(Name = "לחיפוש מהיר מספר  הזמנה ")]
        public long FastSearch { get; set; }
    }
}