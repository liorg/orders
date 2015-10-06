using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        //[Required]
        //[Display(Name = "ארגון")]
        //public Guid OrgId { get; set; }

        [Required]
        [Display(Name = "האם אתה שליח?")]
        public bool IsDeliveryBoy { get; set; }

        [Display(Name = "זכור אותי?")]
        public bool RememberMe { get; set; }


    }
}