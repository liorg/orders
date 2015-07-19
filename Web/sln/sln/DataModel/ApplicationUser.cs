using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Organization Organization { get; set; }

        public bool? IsActive
        {
            get;
            set;
        }
       
        public string Department { get; set; }

        public string Subdivision { get; set; }
        
    }
}