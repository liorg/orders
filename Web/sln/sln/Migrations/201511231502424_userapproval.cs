namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userapproval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "PriceValueException", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "GrantUserManager", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "GrantUserManager");
            DropColumn("dbo.Organizations", "PriceValueException");
        }
    }
}
