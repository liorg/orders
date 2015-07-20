using sln.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "בדוק סיסמא שוב")]
        [Compare("Password", ErrorMessage = "הסיסמא חייבת להיות תואמת")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "דוא''ל")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "ארגון")]
        public Guid OrgId { get; set; }

        // Return a pre-poulated instance of AppliationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
            };
            return user;
        }

    }
}