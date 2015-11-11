namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dddxxs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyMessages", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotifyMessages", "IsRead");
        }
    }
}
