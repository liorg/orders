namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        PriceListId = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Name = c.String(),
                        Desc = c.String(),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        Organizations_OrgId = c.Guid(),
                        Product_ProductId = c.Guid(),
                    })
                .PrimaryKey(t => t.PriceListId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .Index(t => t.Organizations_OrgId)
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Guid(nullable: false),
                        Name = c.String(),
                        ProductNumber = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceLists", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.PriceLists", "Organizations_OrgId", "dbo.Organizations");
            DropIndex("dbo.PriceLists", new[] { "Product_ProductId" });
            DropIndex("dbo.PriceLists", new[] { "Organizations_OrgId" });
            DropTable("dbo.Products");
            DropTable("dbo.PriceLists");
        }
    }
}
