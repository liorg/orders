namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prudctOrgManyToMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductOrganizations",
                c => new
                    {
                        Product_ProductId = c.Guid(nullable: false),
                        Organization_OrgId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ProductId, t.Organization_OrgId })
                .ForeignKey("dbo.Products", t => t.Product_ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId, cascadeDelete: true)
                .Index(t => t.Product_ProductId)
                .Index(t => t.Organization_OrgId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOrganizations", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.ProductOrganizations", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.ProductOrganizations", new[] { "Organization_OrgId" });
            DropIndex("dbo.ProductOrganizations", new[] { "Product_ProductId" });
            DropTable("dbo.ProductOrganizations");
        }
    }
}
