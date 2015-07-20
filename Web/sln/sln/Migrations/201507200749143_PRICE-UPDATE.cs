namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PRICEUPDATE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "Price", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Shippings", "TimeWait", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Shippings", "EstimatedPrice", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "EstimatedPrice");
            DropColumn("dbo.Shippings", "TimeWait");
            DropColumn("dbo.Shippings", "Price");
        }
    }
}
