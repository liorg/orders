﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ShippingVm
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "סטאטוס")]
        public string Status { get; set; }

        [Display(Name = "סטאטוס")]
        public Guid StatusId { get; set; }

        [Display(Name = "חישוב היעד")]
        public Guid DistanceId { get; set; }

        [Display(Name = "תאריך יצירה ")]
        public string CreatedOn { get; set; }

        [Display(Name = "נוצר ע''י ")]
        public string CreatedBy { get; set; }

        [Display(Name = "חישוב היעד")]
        public string DistanceName { get; set; }

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

        //[Display(Name = "מקור")]
        //public string From
        //{
        //    get
        //    {
        //        return SreetFrom + " " + NumFrom + " " + CityFormName;
        //    }
        //}

        //[Display(Name = "יעד")]
        //public string To
        //{
        //    get
        //    {
        //        return NameTarget + " " + SreetTo + " " + NumTo + " " + CityToName;
        //    }
        //}
        [Display(Name = "כתובת יעד")]
        public AddressEditorViewModel TargetAddress { get; set; }

        [Display(Name = "כתובת מקור")]
        public AddressEditorViewModel SourceAddress { get; set; }
    }
}