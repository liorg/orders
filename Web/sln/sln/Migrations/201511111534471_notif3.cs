namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notif3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserNotifications");
            AlterColumn("dbo.UserNotifications", "DeviceId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserNotifications", new[] { "DeviceId", "UserId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UserNotifications");
            AlterColumn("dbo.UserNotifications", "DeviceId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.UserNotifications", new[] { "DeviceId", "UserId" });
        }
    }
}
