namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SyncTable",
                c => new
                    {
                        SyncTableId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        DeviceId = c.String(),
                        ClientId = c.String(),
                        LastUpdateRecord = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        SyncStatus = c.Int(nullable: false),
                        ObjectTableCode = c.Int(nullable: false),
                        ObjectId = c.Guid(nullable: false),
                        SyncStateRecord = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SyncTableId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SyncTable");
        }
    }
}
