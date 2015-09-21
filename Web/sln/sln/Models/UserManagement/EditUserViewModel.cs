using Michal.Project.Contract;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class EditUserViewModel : IRole
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
            this.Tel = user.Tel;
            this.OrgId = user.Organization_OrgId.GetValueOrDefault();
            this.Address = new AddressEditorViewModel();
            this.Address.City = user.AddressUser.CityName;
            this.Address.Citycode = user.AddressUser.CityCode;
            this.Address.Street = user.AddressUser.StreetName;
            this.Address.Streetcode = user.AddressUser.StreetCode;
            this.Address.UId = user.AddressUser.UID;
            this.Address.ExtraDetail = user.AddressUser.ExtraDetail;
            this.Address.Num = user.AddressUser.StreetNum;

        }
        [Display(Name = "כתובת לקוח")]
        public AddressEditorViewModel Address { get; set; }

        [Display(Name = "מספר עובד")]
        public string EmpId { get; set; }
        // [Required]
        [Display(Name = "מזהה")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "שם פרטי שדה חובה")]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "שם משפחה שדה חובה")]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "שם דוא''ל שדה חובה")]
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


        [Display(Name = "משתמש פעיל?")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "טלפון שדה חובה")]
        [Display(Name = "טלפון")]
        [RegularExpression(@"^\d+$", ErrorMessage = "מספר טלפון מכיל מספרים בלבד")]
        public string Tel { get; set; }
    }
}