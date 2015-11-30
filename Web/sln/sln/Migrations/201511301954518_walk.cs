namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class walk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "WalkOrder", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsClientUser", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsClientUser");
            DropColumn("dbo.Shippings", "WalkOrder");
        }
    }
}
