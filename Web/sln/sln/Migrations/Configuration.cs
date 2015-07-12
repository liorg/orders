namespace sln.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using sln.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<sln.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(sln.Models.ApplicationDbContext context)
        {
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
    }
}
