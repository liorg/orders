namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lead : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        LeadId = c.Guid(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Tel = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LeadId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leads");
        }
    }
}
