namespace sln.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using sln.Dal;
    using sln.DataModel;
    using sln.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
           AddUserAndRoles();
            //var manager = new UserManager<ApplicationUser>(
            //      new UserStore<ApplicationUser>(
            //          new ApplicationDbContext()));
            //var user = new ApplicationUser
            //{
            //    UserName = "a",
            //   // PasswordHash = "1"
            //}; 
            //manager.Create(user, "a");
            //for (int i = 0; i < 4; i++)
            //{
            //    var user = new ApplicationUser()
            //    {
            //        UserName = string.Format("User{0}", i.ToString())
            //    };
            //    manager.Create(user, string.Format("Password{0}", i.ToString()));
            //}
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

      
        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();
            //success = idManager.CreateRole("Admin");
            //if (!success == true) return success;

            //success = idManager.CreateRole("CanEdit");
            //if (!success == true) return success;

            //success = idManager.CreateRole("User");
            //if (!success) return success;


            var newUser = new ApplicationUser()
            {
                UserName = "r",
                FirstName = "עבדיאן",
                LastName = "רן",
                Email = "r@rw.com",
                IsActive=true
            };

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(newUser, "Aa123456");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "Admin");
            if (!success) return success;

            //success = idManager.AddUserToRole(newUser.Id, "CanEdit");
            //if (!success) return success;

            //success = idManager.AddUserToRole(newUser.Id, "User");
            //if (!success) return success;

            return success;
        }
    }
}
