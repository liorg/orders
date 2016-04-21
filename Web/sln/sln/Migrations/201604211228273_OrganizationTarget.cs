namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationTarget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "OrganizationTarget_OrgId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "OrganizationTarget_OrgId");
        }
    }
}
