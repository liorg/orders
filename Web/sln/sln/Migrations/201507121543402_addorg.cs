namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addorg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrgId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.OrgId);
            
            AddColumn("dbo.AspNetUsers", "Organization_OrgId", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "Organization_OrgId");
            AddForeignKey("dbo.AspNetUsers", "Organization_OrgId", "dbo.Organizations", "OrgId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Organization_OrgId", "dbo.Organizations");
            DropIndex("dbo.AspNetUsers", new[] { "Organization_OrgId" });
            DropColumn("dbo.AspNetUsers", "Organization_OrgId");
            DropTable("dbo.Organizations");
        }
    }
}
