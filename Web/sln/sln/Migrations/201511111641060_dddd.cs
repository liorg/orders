namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dddd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNotifies",
                c => new
                    {
                        UserNotifyId = c.Guid(nullable: false),
                        DeviceId = c.String(),
                        UserId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserNotifyId);
            
            DropTable("dbo.UserNotifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        DeviceId = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeviceId, t.UserId });
            
            DropTable("dbo.UserNotifies");
        }
    }
}
