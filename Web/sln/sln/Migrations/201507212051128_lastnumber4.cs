namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastnumber4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.XbzCounters",
                c => new
                    {
                        XbzCounterId = c.Guid(nullable: false),
                        Name = c.String(),
                        LastNumber = c.Long(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.XbzCounterId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.XbzCounters");
        }
    }
}
