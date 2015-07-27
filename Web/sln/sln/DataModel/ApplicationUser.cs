using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
       
        public Guid? Organization_OrgId { get; set; }
       
        public Organization Organization { get; set; }

       // [Required]
        public bool IsActive
        {
            get;
            set;
        }
       
        public string Department { get; set; }

        public string Subdivision { get; set; }

        public string EmpId { get; set; }
        
    }
}