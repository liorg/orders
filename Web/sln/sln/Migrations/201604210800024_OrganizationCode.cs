namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "OrganizationCode", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "OrganizationCode");
        }
    }
}
