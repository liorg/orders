using Microsoft.AspNet.Identity.EntityFramework;
using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class ApplicationUser : IdentityUser, IViewerUser
    {
        public ApplicationUser()
        {
            FollowsBy = new HashSet<Shipping>();
            AddressUser = new Address();
        }

        public ApplicationUser(string userName)
            : base(userName)
        {
            FollowsBy = new HashSet<Shipping>();
            AddressUser = new Address();
        }

        public Address AddressUser { get; set; }
        // many to many
        public ICollection<Shipping> FollowsBy { get; set; }

        //http://www.entityframeworktutorial.net/code-first/foreignkey-dataannotations-attribute-in-code-first.aspx
        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Tel { get; set; }

        public Guid? Organization_OrgId { get; set; }

        public Organization Organization { get; set; }

        public bool IsClientUser { get; set; }
        // [Required]
        public bool IsActive
        {
            get;
            set;
        }

        public string Department { get; set; }

        public string Subdivision { get; set; }

        public Guid? GrantUserManager { get; set; }

        public string EmpId { get; set; }

        public int DefaultView { get; set; }

        public bool ViewAll { get; set; }


    }
}