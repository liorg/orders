using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class AddressEditorViewModel
    {
        public int? UId { get; set; }

        [Required(ErrorMessage = "כתובת שדה חובה")]
        [Display(Name = "כתובת")]
        public string Street { get; set; }

        [Required(ErrorMessage = "כתובת שדה חובה")]
        public string Streetcode { get; set; }

        [Required(ErrorMessage = "עיר שדה חובה")]
        [Display(Name = "ישוב")]
        public string City { get; set; }

        [Required(ErrorMessage = "עיר שדה חובה")]
        public string Citycode { get; set; }

        [Required(ErrorMessage = "מספר בית שדה חובה")]
        [Display(Name = "מספר בית")]
        public string Num { get; set; }
        public string NumOld { get; set; }

        [Display(Name = "פרטים נוספים")]
        public string ExtraDetail { get; set; }

        public string CitycodeOld { get; set; }
        public string StreetcodeOld { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool IsSensor { get; set; }

        public override string ToString()
        {
            return this.Street + " " + this.Num + "," + this.City;
        }
    }
}