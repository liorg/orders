namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fg3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Organization_OrgId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Organization_OrgId1", newName: "Organization_OrgId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Organization_OrgId", newName: "Organization_OrgId1");
            AddColumn("dbo.AspNetUsers", "Organization_OrgId", c => c.Guid());
        }
    }
}
