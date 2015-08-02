namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShippingItemPrices", "ShippingItem_ShippingItemId", "dbo.ShippingItems");
            DropIndex("dbo.ShippingItemPrices", new[] { "ShippingItem_ShippingItemId" });
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Guid(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .Index(t => t.Shipping_ShippingId);
            
            AddColumn("dbo.Shippings", "BroughtShippingSender", c => c.Guid());
            AddColumn("dbo.Shippings", "BroughtShipmentCustomer", c => c.Guid());
            DropColumn("dbo.Shippings", "BroughtShipping");
            DropTable("dbo.ShippingItemPrices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShippingItemPrices",
                c => new
                    {
                        ShippingItemPriceId = c.Guid(nullable: false),
                        ShippingItem_ShippingItemId = c.Guid(),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingItemPriceId);
            
            AddColumn("dbo.Shippings", "BroughtShipping", c => c.Guid());
            DropForeignKey("dbo.Comments", "Shipping_ShippingId", "dbo.Shippings");
            DropIndex("dbo.Comments", new[] { "Shipping_ShippingId" });
            DropColumn("dbo.Shippings", "BroughtShipmentCustomer");
            DropColumn("dbo.Shippings", "BroughtShippingSender");
            DropTable("dbo.Comments");
            CreateIndex("dbo.ShippingItemPrices", "ShippingItem_ShippingItemId");
            AddForeignKey("dbo.ShippingItemPrices", "ShippingItem_ShippingItemId", "dbo.ShippingItems", "ShippingItemId");
        }
    }
}
