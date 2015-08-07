﻿using System;
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

        [Required(ErrorMessage = "כתובת מקור חובה")]
        [Display(Name = "כתובת מקור")]
        public string SreetFrom { get; set; }

        [Display(Name = "מספר כתובת מקור ")]
        [Required(ErrorMessage = "מספר כתובת מקור חובה")]
        public string NumFrom { get; set; }

        [Required(ErrorMessage = "כתובת יעד חובה")]
        [Display(Name = "כתובת יעד ")]
        public string SreetTo { get; set; }

        [Required(ErrorMessage = "מספר כתובת יעד חובה")]
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

        [Display(Name = "תאריך עדכון ")]
        public string ModifiedOn { get; set; }

        [Display(Name = "סוג משלוח")]
        public string ShipTypeIdName { get; set; }

        [Display(Name = "סוג משלוח")]
        public Guid ShipTypeId { get; set; }

        [Display(Name = "שם איש קשר יעד")]
        [Required(ErrorMessage = "שם איש קשר חובה")]
        public string Recipient { get; set; }

      
        public string ActualRecipient { get; set; }

        [Display(Name = "טלפון של המזמין")]

        public string TelSource { get; set; }

        [Display(Name = "טלפון של המקבל")]
        [Required(ErrorMessage = "טלפון שדה חובה")]
        [RegularExpression(@"^\d+$", ErrorMessage = "מספר טלפון מכיל מספרים בלבד")]
        public string TelTarget { get; set; }

        [Display(Name = "שם של המזמין")]
        public string NameSource { get; set; }

        [Display(Name = "שם המקבל")]
        [Required(ErrorMessage = "שם המקבל שדה חובה")]
        public string NameTarget { get; set; }

        public string From
        {
            get
            {
                return SreetFrom + " " + NumFrom + " " + CityFormName;
            }
        }

        public string To
        {
            get
            {
                return NameTarget +" "+SreetTo + " " + NumTo + " " + CityToName;
            }
        }
    }
}