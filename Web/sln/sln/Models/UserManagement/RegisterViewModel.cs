using Michal.Project.Contract;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class RegisterViewModel : IRole
    {
        [Display(Name = "כתובת לקוח")]
        public AddressEditorViewModel Address { get; set; }

        [Display(Name = "מספר עובד")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "שם משתמש שדה חובה")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "הזן אותיות אנגליות בלבד ללא רווחים")]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "סיסמא שדה חובה")]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "בדוק סיסמא שוב")]
        [Compare("Password", ErrorMessage = "הסיסמא חייבת להיות תואמת")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "שם פרטי שדה חובה")]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "שם משפחה שדה חובה")]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "שם דוא''ל שדה חובה")]
        [Display(Name = "דוא''ל")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ארגון שדה חובה")]
        [Display(Name = "ארגון")]
        public Guid OrgId { get; set; }


        [Display(Name = "תפקיד מנהל מערכת")]
        public bool IsAdmin { get; set; }

        [Display(Name = "תפקיד מנהל ארגוני")]
        public bool IsOrgMangager { get; set; }

        [Display(Name = "תפקיד שליח")]
        public bool IsRunner { get; set; }

        [Display(Name = "תפקיד יוצר הזמנה")]
        public bool IsCreateOrder { get; set; }

        [Display(Name = "תפקיד מאשר הזמנה")]
        public bool IsAcceptOrder { get; set; }

        [Display(Name = "תפקיד מאשר חריגות")]
        public bool IsApprovalExceptionalBudget { get; set; }

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
        [Required(ErrorMessage = "טלפון שדה חובה")]
        [Display(Name = "טלפון")]
        [RegularExpression(@"^\d+$", ErrorMessage = "מספר טלפון מכיל מספרים בלבד")]
        public string Tel { get; set; }

          [Display(Name = "מנהל מאשר חריגות")]
        public Guid? GrantUserManager { get; set; }
    }
}