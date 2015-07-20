namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cityadd : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.Shippings", "CityFrom_CityId", c => c.Guid());
            AddColumn("dbo.Shippings", "CityTo_CityId", c => c.Guid());
            CreateIndex("dbo.Shippings", "CityFrom_CityId");
            CreateIndex("dbo.Shippings", "CityTo_CityId");
            AddForeignKey("dbo.Shippings", "CityFrom_CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Shippings", "CityTo_CityId", "dbo.Cities", "CityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shippings", "CityTo_CityId", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "CityFrom_CityId", "dbo.Cities");
            DropIndex("dbo.Shippings", new[] { "CityTo_CityId" });
            DropIndex("dbo.Shippings", new[] { "CityFrom_CityId" });
            DropColumn("dbo.Shippings", "CityTo_CityId");
            DropColumn("dbo.Shippings", "CityFrom_CityId");
            DropTable("dbo.Cities");
        }
    }
}
