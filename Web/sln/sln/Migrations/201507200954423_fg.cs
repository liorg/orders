namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fg : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Organization_OrgId", "dbo.Organizations");
            DropIndex("dbo.AspNetUsers", new[] { "Organization_OrgId" });
            AddColumn("dbo.AspNetUsers", "Organization_OrgId1", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "Organization_OrgId1");
            AddForeignKey("dbo.AspNetUsers", "Organization_OrgId1", "dbo.Organizations", "OrgId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Organization_OrgId1", "dbo.Organizations");
            DropIndex("dbo.AspNetUsers", new[] { "Organization_OrgId1" });
            DropColumn("dbo.AspNetUsers", "Organization_OrgId1");
            CreateIndex("dbo.AspNetUsers", "Organization_OrgId");
            AddForeignKey("dbo.AspNetUsers", "Organization_OrgId", "dbo.Organizations", "OrgId");
        }
    }
}
