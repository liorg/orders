namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingItems", "Quantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShippingItems", "Quantity");
        }
    }
}
