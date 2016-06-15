namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sss2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShippingCompanyDistances",
                c => new
                    {
                        ShippingCompany_ShippingCompanyId = c.Guid(nullable: false),
                        Distance_DistanceId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShippingCompany_ShippingCompanyId, t.Distance_DistanceId })
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId, cascadeDelete: true)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Distance_DistanceId);
            
            CreateTable(
                "dbo.ProductShippingCompanies",
                c => new
                    {
                        Product_ProductId = c.Guid(nullable: false),
                        ShippingCompany_ShippingCompanyId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ProductId, t.ShippingCompany_ShippingCompanyId })
                .ForeignKey("dbo.Products", t => t.Product_ProductId, cascadeDelete: true)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId, cascadeDelete: true)
                .Index(t => t.Product_ProductId)
                .Index(t => t.ShippingCompany_ShippingCompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductShippingCompanies", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.ProductShippingCompanies", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.ShippingCompanyDistances", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.ShippingCompanyDistances", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropIndex("dbo.ProductShippingCompanies", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.ProductShippingCompanies", new[] { "Product_ProductId" });
            DropIndex("dbo.ShippingCompanyDistances", new[] { "Distance_DistanceId" });
            DropIndex("dbo.ShippingCompanyDistances", new[] { "ShippingCompany_ShippingCompanyId" });
            DropTable("dbo.ProductShippingCompanies");
            DropTable("dbo.ShippingCompanyDistances");
        }
    }
}
