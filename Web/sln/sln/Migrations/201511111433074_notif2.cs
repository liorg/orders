namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notif2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotifyMessages",
                c => new
                    {
                        NotifyMessageId = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        ToUrl = c.String(),
                        userId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NotifyMessageId);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        DeviceId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeviceId, t.UserId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserNotifications");
            DropTable("dbo.NotifyMessages");
        }
    }
}
