namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dddd1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingCompanies", "ManagerId", c => c.Guid());
            AddColumn("dbo.RequestShippings", "Total", c => c.Decimal(nullable: false, storeType: "money"));
            DropColumn("dbo.Shippings", "TimeWait");
            DropColumn("dbo.Shippings", "EstimatedPrice");
            DropColumn("dbo.Shippings", "Runner");
            DropColumn("dbo.Shippings", "CloseDesc");
            DropColumn("dbo.RequestItemShips", "PriceClientValueType");
            DropColumn("dbo.RequestItemShips", "PriceClientValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestItemShips", "PriceClientValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RequestItemShips", "PriceClientValueType", c => c.Int(nullable: false));
            AddColumn("dbo.Shippings", "CloseDesc", c => c.String());
            AddColumn("dbo.Shippings", "Runner", c => c.Guid());
            AddColumn("dbo.Shippings", "EstimatedPrice", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Shippings", "TimeWait", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.RequestShippings", "Total");
            DropColumn("dbo.ShippingCompanies", "ManagerId");
        }
    }
}
