namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class distance_many_to_many : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationDistances",
                c => new
                    {
                        Organization_OrgId = c.Guid(nullable: false),
                        Distance_DistanceId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_OrgId, t.Distance_DistanceId })
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId, cascadeDelete: true)
                .Index(t => t.Organization_OrgId)
                .Index(t => t.Distance_DistanceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationDistances", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.OrganizationDistances", "Organization_OrgId", "dbo.Organizations");
            DropIndex("dbo.OrganizationDistances", new[] { "Distance_DistanceId" });
            DropIndex("dbo.OrganizationDistances", new[] { "Organization_OrgId" });
            DropTable("dbo.OrganizationDistances");
        }
    }
}
