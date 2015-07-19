using Microsoft.AspNet.Identity.EntityFramework;
using sln.DataModel;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace sln.Dal
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Distance> Distance { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<Product> Product { get; set; }

        
    }
}