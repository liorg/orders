using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class StatusVm
    {
        public Guid StatusId { get; set; }

        public string Name { get; set; }

        public AlertStyle MessageType { get; set; }

        public string Message { get; set; }

        public Guid ShipId { get; set; }
        [Required]
        [Display(Name = "שם איש הקשר")]
        public string Recipient { get; set; }

        [Required]
        [Display(Name = "טלפון של איש הקשר")]
        public string TelRecipient { get; set; }

        [Required]
        [Display(Name = "שם המקבל")]
        public string NameTarget { get; set; }

        public string PicBase64 { get; set; }

        public List<Runner> Runners { get; set; }

        public string PathSig { get; set; }

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