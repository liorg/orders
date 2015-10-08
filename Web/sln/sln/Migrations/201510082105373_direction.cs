namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class direction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "Direction", c => c.Int(nullable: false));
            AddColumn("dbo.Shippings", "TimeWaitStartSend", c => c.DateTime());
            AddColumn("dbo.Shippings", "TimeWaitEndSend", c => c.DateTime());
            AddColumn("dbo.Shippings", "TimeWaitStartSGet", c => c.DateTime());
            AddColumn("dbo.Shippings", "TimeWaitEndGet", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "TimeWaitEndGet");
            DropColumn("dbo.Shippings", "TimeWaitStartSGet");
            DropColumn("dbo.Shippings", "TimeWaitEndSend");
            DropColumn("dbo.Shippings", "TimeWaitStartSend");
            DropColumn("dbo.Shippings", "Direction");
        }
    }
}
