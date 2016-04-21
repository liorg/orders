namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        MemberId = c.Guid(nullable: false),
                        UserId1 = c.Guid(nullable: false),
                        UserId2 = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MemberId)
                .Index(t => new { t.UserId1, t.UserId2 }, unique: true, name: "IX_UserId");
            
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Member", "IX_UserId");
        
        }
    }
}
