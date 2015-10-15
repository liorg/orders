namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestShippings", "Shipping_ShippingId", c => c.Guid());
            CreateIndex("dbo.RequestShippings", "Shipping_ShippingId");
            AddForeignKey("dbo.RequestShippings", "Shipping_ShippingId", "dbo.Shippings", "ShippingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestShippings", "Shipping_ShippingId", "dbo.Shippings");
            DropIndex("dbo.RequestShippings", new[] { "Shipping_ShippingId" });
            DropColumn("dbo.RequestShippings", "Shipping_ShippingId");
        }
    }
}
