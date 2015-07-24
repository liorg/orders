namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipp : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ShippingItemPriceId)
                .ForeignKey("dbo.ShippingItems", t => t.ShippingItem_ShippingItemId)
                .Index(t => t.ShippingItem_ShippingItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingItemPrices", "ShippingItem_ShippingItemId", "dbo.ShippingItems");
            DropIndex("dbo.ShippingItemPrices", new[] { "ShippingItem_ShippingItemId" });
            DropTable("dbo.ShippingItemPrices");
        }
    }
}
