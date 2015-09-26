namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sla : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Slas",
                c => new
                    {
                        SlaId = c.Guid(nullable: false),
                        Priority = c.Int(nullable: false),
                        IsBusinessDay = c.Boolean(nullable: false),
                        Days = c.Double(nullable: false),
                        Hours = c.Double(nullable: false),
                        Mins = c.Double(nullable: false),
                        ShipType_ShipTypeId = c.Guid(),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        Distance_DistanceId = c.Guid(),
                        Organizations_OrgId = c.Guid(),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SlaId)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .ForeignKey("dbo.ShipTypes", t => t.ShipType_ShipTypeId)
                .Index(t => t.ShipType_ShipTypeId)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Distance_DistanceId)
                .Index(t => t.Organizations_OrgId);
            
            AddColumn("dbo.Shippings", "SlaTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Slas", "ShipType_ShipTypeId", "dbo.ShipTypes");
            DropForeignKey("dbo.Slas", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.Slas", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Slas", "Distance_DistanceId", "dbo.Distances");
            DropIndex("dbo.Slas", new[] { "Organizations_OrgId" });
            DropIndex("dbo.Slas", new[] { "Distance_DistanceId" });
            DropIndex("dbo.Slas", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.Slas", new[] { "ShipType_ShipTypeId" });
            DropColumn("dbo.Shippings", "SlaTime");
            DropTable("dbo.Slas");
        }
    }
}
