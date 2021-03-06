﻿using Microsoft.AspNet.Identity.EntityFramework;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Michal.Project.Dal
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
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<ShippingItem> ShippingItem { get; set; }
        public DbSet<RequestShipping> RequestShipping { get; set; }
        public DbSet<StatusShipping> StatusShipping { get; set; }
        public DbSet<TimeLine> TimeLine { get; set; }
        public DbSet<Sla> Sla { get; set; }
        public DbSet<XbzCounter> XbzCounter { get; set; }
        public DbSet<Lead> Lead { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<ShipType> ShipType { get; set; }
        public DbSet<ShippingCompany> ShippingCompany { get; set; }
        public DbSet<TableTest> TableTest { get; set; }
        public DbSet<AttachmentShipping> AttachmentShipping { get; set; }
        public DbSet<ProductSystem> ProductSystem { get; set; }
        public DbSet<RequestItemShip> RequestItemShip { get; set; }
        public DbSet<NotifyMessage> NotifyMessage { get; set; }
        public DbSet<BussinessClosure> BussinessClosure { get; set; }
        public DbSet<DistanceCities> DistanceCities { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<UserNotify> UserNotify { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<SyncTable> SyncTable { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<MoreAddress> MoreAddress { get; set; }

        public DbSet<Friend> Friends { get; set; }
    }
}