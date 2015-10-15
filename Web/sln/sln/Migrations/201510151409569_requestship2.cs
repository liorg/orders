namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestship2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PriceLists", "ShippingCompany_ShippingCompanyId");
            RenameColumn(table: "dbo.PriceLists", name: "ShippingCompany_ShippingCompanyId1", newName: "ShippingCompany_ShippingCompanyId");
            RenameIndex(table: "dbo.PriceLists", name: "IX_ShippingCompany_ShippingCompanyId1", newName: "IX_ShippingCompany_ShippingCompanyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PriceLists", name: "IX_ShippingCompany_ShippingCompanyId", newName: "IX_ShippingCompany_ShippingCompanyId1");
            RenameColumn(table: "dbo.PriceLists", name: "ShippingCompany_ShippingCompanyId", newName: "ShippingCompany_ShippingCompanyId1");
            AddColumn("dbo.PriceLists", "ShippingCompany_ShippingCompanyId", c => c.Guid());
        }
    }
}
