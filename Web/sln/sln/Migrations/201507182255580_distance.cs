namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class distance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Distances",
                c => new
                    {
                        DistanceId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DistanceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Distances");
        }
    }
}
