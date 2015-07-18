﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [Display(Name = "זכור אותי?")]
        public bool RememberMe { get; set; }

        //[Required]
        //[Display(Name = "שם פרטי")]
        //public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "שם משפחה")]
        //public string LastName { get; set; }

        //[Required]
        //[Display(Name = "דוא''ל")]
        //public string Email { get; set; }

    }
}