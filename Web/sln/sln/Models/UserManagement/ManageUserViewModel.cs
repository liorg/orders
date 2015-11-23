using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ManageUserViewModel
    {
       
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא נוכחית")]
        public string OldPassword { get; set; }

        [Required]
        //  [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא חדשה")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "אישור סיסמא חדשה")]
        [Compare("NewPassword", ErrorMessage = "הסיסמא החדשה אמורה להיות תואמת ")]
        public string ConfirmPassword { get; set; }
    }

}