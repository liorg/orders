namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "ActualPrice", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Shippings", "CancelByUser", c => c.Guid());
            AddColumn("dbo.Shippings", "CancelByAdmin", c => c.Guid());
            AddColumn("dbo.Shippings", "ArrivedShippingSender", c => c.Guid());
            AddColumn("dbo.Shippings", "ClosedShippment", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "ClosedShippment");
            DropColumn("dbo.Shippings", "ArrivedShippingSender");
            DropColumn("dbo.Shippings", "CancelByAdmin");
            DropColumn("dbo.Shippings", "CancelByUser");
            DropColumn("dbo.Shippings", "ActualPrice");
        }
    }
}
