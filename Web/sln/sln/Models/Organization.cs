using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class Organization
    {
        public Organization()
        {
            Users = new HashSet<ApplicationUser>();
        }
        [Key]
        public Guid OrgId { get; set; }

        public string Name { get; set; }

        public ICollection<ApplicationUser> Users {get;set;}
    }
}