namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dddxxaX : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityCode = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityCode);
            
            CreateTable(
                "dbo.DistanceCities",
                c => new
                    {
                        CityCode1 = c.String(nullable: false, maxLength: 128),
                        CityCode2 = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Desc = c.String(),
                        DestinationAddress = c.String(),
                        OriginAddress = c.String(),
                        DestinationLat = c.Double(nullable: false),
                        DestinationLng = c.Double(nullable: false),
                        OriginLat = c.Double(nullable: false),
                        OriginLng = c.Double(nullable: false),
                        DistanceValue = c.Single(nullable: false),
                        DistanceText = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.CityCode1, t.CityCode2 });
            
            AddColumn("dbo.Distances", "FromDistance", c => c.Single());
            AddColumn("dbo.Distances", "ToDistance", c => c.Single());
            AddColumn("dbo.Organizations", "Perfix", c => c.String());
            AddColumn("dbo.XbzCounters", "Organizations_OrgId", c => c.Guid());
            CreateIndex("dbo.XbzCounters", "Organizations_OrgId");
            AddForeignKey("dbo.XbzCounters", "Organizations_OrgId", "dbo.Organizations", "OrgId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.XbzCounters", "Organizations_OrgId", "dbo.Organizations");
            DropIndex("dbo.XbzCounters", new[] { "Organizations_OrgId" });
            DropColumn("dbo.XbzCounters", "Organizations_OrgId");
            DropColumn("dbo.Organizations", "Perfix");
            DropColumn("dbo.Distances", "ToDistance");
            DropColumn("dbo.Distances", "FromDistance");
            DropTable("dbo.DistanceCities");
            DropTable("dbo.Cities");
        }
    }
}
