using sln.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(ApplicationUser user)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.IsActive = user.IsActive;
            this.EmpId = user.EmpId;
            this.OrgId = user.Organization_OrgId.HasValue ? user.Organization_OrgId.Value : Guid.Empty;
            
        }
        [Display(Name = "מספר עובד")]
        public string EmpId { get; set; }
       // [Required]
        [Display(Name = "מזהה")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

       // [Required]
        [Display(Name = "דואל")]
        public string Email { get; set; }

        [Required]
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


        [Display(Name = "משתמש פעיל?")]
        public bool IsActive { get; set; }
    }
}