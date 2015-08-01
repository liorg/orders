using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sln.DataModel;
using sln.Helper;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Dal
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