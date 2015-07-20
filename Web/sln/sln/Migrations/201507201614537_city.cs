namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class city : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shippings", "CityFrom_CityId1", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "CityTo_CityId1", "dbo.Cities");
            DropIndex("dbo.Shippings", new[] { "CityFrom_CityId1" });
            DropIndex("dbo.Shippings", new[] { "CityTo_CityId1" });
            DropColumn("dbo.Shippings", "CityFrom_CityId");
            DropColumn("dbo.Shippings", "CityTo_CityId");
            DropColumn("dbo.Shippings", "CityFrom_CityId1");
            DropColumn("dbo.Shippings", "CityTo_CityId1");
            DropTable("dbo.Cities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId);
            
            AddColumn("dbo.Shippings", "CityTo_CityId1", c => c.Guid());
            AddColumn("dbo.Shippings", "CityFrom_CityId1", c => c.Guid());
            AddColumn("dbo.Shippings", "CityTo_CityId", c => c.Guid());
            AddColumn("dbo.Shippings", "CityFrom_CityId", c => c.Guid());
            CreateIndex("dbo.Shippings", "CityTo_CityId1");
            CreateIndex("dbo.Shippings", "CityFrom_CityId1");
            AddForeignKey("dbo.Shippings", "CityTo_CityId1", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Shippings", "CityFrom_CityId1", "dbo.Cities", "CityId");
        }
    }
}
