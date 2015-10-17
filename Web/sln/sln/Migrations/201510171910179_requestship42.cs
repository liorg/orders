namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestship42 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiscountRequestShippings", "Discount_DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.DiscountRequestShippings", "RequestShipping_RequestShippingId", "dbo.RequestShippings");
            DropIndex("dbo.DiscountRequestShippings", new[] { "Discount_DiscountId" });
            DropIndex("dbo.DiscountRequestShippings", new[] { "RequestShipping_RequestShippingId" });
            CreateTable(
                "dbo.RequestItemShips",
                c => new
                    {
                        RequestItemShipId = c.Guid(nullable: false),
                        RequestShipping_RequestShippingId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        StatusCode = c.Int(nullable: false),
                        ReqeustType = c.Int(nullable: false),
                        PriceValueType = c.Int(nullable: false),
                        PriceValue = c.Decimal(precision: 18, scale: 2),
                        PriceClientValueType = c.Int(nullable: false),
                        PriceClientValue = c.Decimal(precision: 18, scale: 2),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestItemShipId)
                .ForeignKey("dbo.RequestShippings", t => t.RequestShipping_RequestShippingId)
                .Index(t => t.RequestShipping_RequestShippingId);
            
            DropTable("dbo.DiscountRequestShippings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DiscountRequestShippings",
                c => new
                    {
                        Discount_DiscountId = c.Guid(nullable: false),
                        RequestShipping_RequestShippingId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Discount_DiscountId, t.RequestShipping_RequestShippingId });
            
            DropForeignKey("dbo.RequestItemShips", "RequestShipping_RequestShippingId", "dbo.RequestShippings");
            DropIndex("dbo.RequestItemShips", new[] { "RequestShipping_RequestShippingId" });
            DropTable("dbo.RequestItemShips");
            CreateIndex("dbo.DiscountRequestShippings", "RequestShipping_RequestShippingId");
            CreateIndex("dbo.DiscountRequestShippings", "Discount_DiscountId");
            AddForeignKey("dbo.DiscountRequestShippings", "RequestShipping_RequestShippingId", "dbo.RequestShippings", "RequestShippingId", cascadeDelete: true);
            AddForeignKey("dbo.DiscountRequestShippings", "Discount_DiscountId", "dbo.Discounts", "DiscountId", cascadeDelete: true);
        }
    }
}
