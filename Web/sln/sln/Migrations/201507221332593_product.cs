namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingItems", "Product_ProductId", c => c.Guid());
            CreateIndex("dbo.ShippingItems", "Product_ProductId");
            AddForeignKey("dbo.ShippingItems", "Product_ProductId", "dbo.Products", "ProductId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingItems", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.ShippingItems", new[] { "Product_ProductId" });
            DropColumn("dbo.ShippingItems", "Product_ProductId");
        }
    }
}
