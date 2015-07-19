namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class city3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shippings", "City_CityId", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "City_CityId1", "dbo.Cities");
            DropIndex("dbo.Shippings", new[] { "City_CityId" });
            DropIndex("dbo.Shippings", new[] { "City_CityId1" });
            DropColumn("dbo.Shippings", "City_CityId");
            DropColumn("dbo.Shippings", "City_CityId1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shippings", "City_CityId1", c => c.Guid());
            AddColumn("dbo.Shippings", "City_CityId", c => c.Guid());
            CreateIndex("dbo.Shippings", "City_CityId1");
            CreateIndex("dbo.Shippings", "City_CityId");
            AddForeignKey("dbo.Shippings", "City_CityId1", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Shippings", "City_CityId", "dbo.Cities", "CityId");
        }
    }
}
