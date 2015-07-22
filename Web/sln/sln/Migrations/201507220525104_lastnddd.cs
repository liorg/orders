namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastnddd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TimeLines", "StatusTimeLine_StatusTimeLineId", "dbo.StatusTimeLines");
            DropIndex("dbo.TimeLines", new[] { "StatusTimeLine_StatusTimeLineId" });
            AddColumn("dbo.TimeLines", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.TimeLines", "StatusTimeLine_StatusTimeLineId");
            DropTable("dbo.StatusTimeLines");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StatusTimeLines",
                c => new
                    {
                        StatusTimeLineId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StatusTimeLineId);
            
            AddColumn("dbo.TimeLines", "StatusTimeLine_StatusTimeLineId", c => c.Guid());
            DropColumn("dbo.TimeLines", "Status");
            CreateIndex("dbo.TimeLines", "StatusTimeLine_StatusTimeLineId");
            AddForeignKey("dbo.TimeLines", "StatusTimeLine_StatusTimeLineId", "dbo.StatusTimeLines", "StatusTimeLineId");
        }
    }
}
