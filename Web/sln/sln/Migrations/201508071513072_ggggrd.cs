namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ggggrd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "JobType", c => c.String());
            AddColumn("dbo.Comments", "JobTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "JobTitle");
            DropColumn("dbo.Comments", "JobType");
        }
    }
}
