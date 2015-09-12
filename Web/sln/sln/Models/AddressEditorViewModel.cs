using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class AddressEditorViewModel
    {
        public int UId { get; set; }

        [Required(ErrorMessage = "כתובת שדה חובה")]
        [Display(Name = "כתובת")]
        public string Street { get; set; }

        [Required(ErrorMessage = "כתובת שדה חובה")]
        public string Streetcode { get; set; }

        [Required(ErrorMessage = "עיר שדה חובה")]
        [Display(Name = "עיר")]
        public string City { get; set; }

        [Required(ErrorMessage = "עיר שדה חובה")]
        public string Citycode { get; set; }
    }
}