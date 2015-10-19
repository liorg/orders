namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestship49 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discounts", "BeginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Discounts", "EndDate", c => c.DateTime());
            AddColumn("dbo.Discounts", "Organizations_OrgId", c => c.Guid());
            AddColumn("dbo.Discounts", "ShippingCompany_ShippingCompanyId", c => c.Guid());
            AddColumn("dbo.ProductSystems", "SetDefaultValue", c => c.Boolean());
            CreateIndex("dbo.Discounts", "Organizations_OrgId");
            CreateIndex("dbo.Discounts", "ShippingCompany_ShippingCompanyId");
            AddForeignKey("dbo.Discounts", "Organizations_OrgId", "dbo.Organizations", "OrgId");
            AddForeignKey("dbo.Discounts", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies", "ShippingCompanyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Discounts", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.Discounts", "Organizations_OrgId", "dbo.Organizations");
            DropIndex("dbo.Discounts", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.Discounts", new[] { "Organizations_OrgId" });
            DropColumn("dbo.ProductSystems", "SetDefaultValue");
            DropColumn("dbo.Discounts", "ShippingCompany_ShippingCompanyId");
            DropColumn("dbo.Discounts", "Organizations_OrgId");
            DropColumn("dbo.Discounts", "EndDate");
            DropColumn("dbo.Discounts", "BeginDate");
        }
    }
}
