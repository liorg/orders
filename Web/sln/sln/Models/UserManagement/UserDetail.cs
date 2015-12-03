using Michal.Project.Contract;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class UserDetail : IRole
    {
        public UserDetail() { }

        // Allow Initialization with an instance of ApplicationUser:
        public UserDetail(ApplicationUser user)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.IsActive = user.IsActive;
            this.EmpId = user.EmpId;
            this.Tel = user.Tel;
            this.OrgId = user.Organization_OrgId.GetValueOrDefault();
           
            this.IsClientUser = user.IsClientUser;
            this.CompanyId = user.ShippingCompany_ShippingCompanyId.GetValueOrDefault();
            this.Department = user.Department;

        }
        [Display(Name = "ספק")]
        public Guid CompanyId { get; set; }

        [Display(Name = "יחידה ארגונית")]
        public string Department { get; set; }

        [Display(Name = " האם הוא לקוח")]
        public bool  IsClientUser { get; set; }

      

        [Display(Name = "מספר עובד")]
        public string EmpId { get; set; }
        // [Required]
        [Display(Name = "מזהה")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "שם פרטי")]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "שם משפחה ")]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
              return  this.FirstName + " " + this.LastName;
            }
        }

        [Required(ErrorMessage = "שם דוא''ל ")]
        [Display(Name = "דואל")]
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

        [Display(Name = "משתמש פעיל?")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "טלפון שדה חובה")]
        [Display(Name = "טלפון")]
        [RegularExpression(@"^\d+$", ErrorMessage = "מספר טלפון מכיל מספרים בלבד")]
        public string Tel { get; set; }

        [Display(Name = "מנהל מאשר חריגות")]
        public Guid? GrantUserManager { get; set; }
    }
}