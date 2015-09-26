namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipcompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShippingCompanies",
                c => new
                    {
                        ShippingCompanyId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingCompanyId);
            
            CreateTable(
                "dbo.ShippingCompanyOrganizations",
                c => new
                    {
                        ShippingCompany_ShippingCompanyId = c.Guid(nullable: false),
                        Organization_OrgId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShippingCompany_ShippingCompanyId, t.Organization_OrgId })
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId, cascadeDelete: true)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Organization_OrgId);
            
            AddColumn("dbo.Shippings", "ShippingCompany_ShippingCompanyId", c => c.Guid());
            AddColumn("dbo.PriceLists", "ShippingCompany_ShippingCompanyId", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "ShippingCompany_ShippingCompanyId", c => c.Guid());
            CreateIndex("dbo.Shippings", "ShippingCompany_ShippingCompanyId");
            CreateIndex("dbo.PriceLists", "ShippingCompany_ShippingCompanyId");
            CreateIndex("dbo.AspNetUsers", "ShippingCompany_ShippingCompanyId");
            AddForeignKey("dbo.AspNetUsers", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies", "ShippingCompanyId");
            AddForeignKey("dbo.PriceLists", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies", "ShippingCompanyId");
            AddForeignKey("dbo.Shippings", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies", "ShippingCompanyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shippings", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.PriceLists", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.AspNetUsers", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.ShippingCompanyOrganizations", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.ShippingCompanyOrganizations", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropIndex("dbo.ShippingCompanyOrganizations", new[] { "Organization_OrgId" });
            DropIndex("dbo.ShippingCompanyOrganizations", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.AspNetUsers", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.PriceLists", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.Shippings", new[] { "ShippingCompany_ShippingCompanyId" });
            DropColumn("dbo.AspNetUsers", "ShippingCompany_ShippingCompanyId");
            DropColumn("dbo.PriceLists", "ShippingCompany_ShippingCompanyId");
            DropColumn("dbo.Shippings", "ShippingCompany_ShippingCompanyId");
            DropTable("dbo.ShippingCompanyOrganizations");
            DropTable("dbo.ShippingCompanies");
        }
    }
}
