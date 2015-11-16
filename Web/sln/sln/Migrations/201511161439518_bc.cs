namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BussinessClosures",
                c => new
                    {
                        BussinessClosureId = c.Guid(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        IsDayOff = c.Boolean(nullable: false),
                        SpecialDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BussinessClosureId);
            
            AddColumn("dbo.PriceLists", "IsPublish", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceLists", "IsPublish");
            DropTable("dbo.BussinessClosures");
        }
    }
}
