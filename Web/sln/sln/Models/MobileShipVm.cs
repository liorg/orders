using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class MobileShipVm
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "תאריך התחלה ")]
        public DateTime? ActualStartDateDt { get; set; }

        [Display(Name = "תאריך סיום ")]
        public DateTime? ActualEndDateDt { get; set; }

        [Display(Name = "תאריך SLA ")]
        public DateTime? SlaEndTime { get; set; }

        [Display(Name = "סוג משלוח")]
        public string ShipTypeIdName { get; set; }

        [Display(Name = "סוג משלוח")]
        public Guid ShipTypeId { get; set; }

     
        [Display(Name = "סטאטוס")]
        public string Status { get; set; }

        [Display(Name = "קצב התקדמות")]
        public double StatusPresent { get; set; }

        [Display(Name = "סדר הליכה")]
        public int WalkOrder { get; set; }

        [Display(Name = "תאריך סיום משוער")]
        public string SlaDate { get; set; }

        [Display(Name = "שם איש קשר יעד")]
        [Required(ErrorMessage = "שם איש קשר חובה")]
        public string Recipient { get; set; }

        [Display(Name = "טלפון של המזמין")]
        public string TelSource { get; set; }

        [Display(Name = "טלפון של המקבל")]
        [Required(ErrorMessage = "טלפון שדה חובה")]
        public string TelTarget { get; set; }

        [Display(Name = "שם של המזמין")]
        public string NameSource { get; set; }

        [Display(Name = "שם המקבל")]
        [Required(ErrorMessage = "שם המקבל שדה חובה")]
        public string NameTarget { get; set; }

        [Display(Name = "כתובת יעד")]
        public AddressEditorViewModel TargetAddress { get; set; }

        [Display(Name = "כתובת מקור")]
        public AddressEditorViewModel SourceAddress { get; set; }

        [Display(Name = "החזרת אסמכתא")]
        public int SigBackType { get; set; }

        [Display(Name = "כיוון ")]
        [Required(ErrorMessage = "חובה לבחור כיוון")]
        public int Direction { get; set; }

        public string PathSig { get; set; }

    }

    public class MobileShipStatusVm
    {
        [Display(Name = "מזהה")]
        public Guid StatusId { get; set; }

        public Guid ShipId { get; set; }

        public string PicBase64 { get; set; }

        [Display(Name = "שם המקבל בפועל")]
        public string NameActualTarget { get; set; }

        [Display(Name = "שם איש הקשר בפועל")]
        public string NameActualRecipient { get; set; }

        [Display(Name = "טלפון של איש הקשר בפועל")]
        public string NameActualTelRecipient { get; set; }

        [Display(Name = "סטאטוס קבלת משלוח כן/לא")]
        public bool IsTake { get; set; }

        [Display(Name = "פרטים נוספים")]
        public string Desc { get; set; }

        [Display(Name = "סוג חתימה")]
        public int SigBackType { get; set; }
    }
}