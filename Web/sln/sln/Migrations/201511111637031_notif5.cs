//namespace Michal.Project.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;
    
//    public partial class notif5 : DbMigration
//    {
//        public override void Up()
//        {
//            DropPrimaryKey("dbo.UserNotifications");
//            AddColumn("dbo.UserNotifications", "UserNotificationId", c => c.Guid(nullable: false));
//            AlterColumn("dbo.UserNotifications", "DeviceId", c => c.String());
//            AddPrimaryKey("dbo.UserNotifications", "UserNotificationId");
//        }
        
//        public override void Down()
//        {
//            DropPrimaryKey("dbo.UserNotifications");
//            AlterColumn("dbo.UserNotifications", "DeviceId", c => c.String(nullable: false, maxLength: 128));
//            DropColumn("dbo.UserNotifications", "UserNotificationId");
//            AddPrimaryKey("dbo.UserNotifications", new[] { "DeviceId", "UserId" });
//        }
//    }
//}
