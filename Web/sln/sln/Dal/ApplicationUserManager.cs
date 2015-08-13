using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Dal
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(ApplicationDbContext context)
            : base(new UserStore<ApplicationUser>(context))
        {
            PasswordValidator = new CustomPasswordValidator(1);

        }
    }
}