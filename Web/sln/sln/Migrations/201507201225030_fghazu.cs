namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fghazu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Discounts", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Shippings", "CityFrom_CityId", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "CityTo_CityId", "dbo.Cities");
            DropIndex("dbo.Discounts", new[] { "Distance_DistanceId" });
            DropIndex("dbo.Shippings", new[] { "CityFrom_CityId" });
            DropIndex("dbo.Shippings", new[] { "CityTo_CityId" });
            AddColumn("dbo.Discounts", "Distance_DistanceId1", c => c.Guid());
            AddColumn("dbo.Shippings", "CityFrom_CityId1", c => c.Guid());
            AddColumn("dbo.Shippings", "CityTo_CityId1", c => c.Guid());
            CreateIndex("dbo.Discounts", "Distance_DistanceId1");
            CreateIndex("dbo.Shippings", "CityFrom_CityId1");
            CreateIndex("dbo.Shippings", "CityTo_CityId1");
            AddForeignKey("dbo.Discounts", "Distance_DistanceId1", "dbo.Distances", "DistanceId");
            AddForeignKey("dbo.Shippings", "CityFrom_CityId1", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Shippings", "CityTo_CityId1", "dbo.Cities", "CityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shippings", "CityTo_CityId1", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "CityFrom_CityId1", "dbo.Cities");
            DropForeignKey("dbo.Discounts", "Distance_DistanceId1", "dbo.Distances");
            DropIndex("dbo.Shippings", new[] { "CityTo_CityId1" });
            DropIndex("dbo.Shippings", new[] { "CityFrom_CityId1" });
            DropIndex("dbo.Discounts", new[] { "Distance_DistanceId1" });
            DropColumn("dbo.Shippings", "CityTo_CityId1");
            DropColumn("dbo.Shippings", "CityFrom_CityId1");
            DropColumn("dbo.Discounts", "Distance_DistanceId1");
            CreateIndex("dbo.Shippings", "CityTo_CityId");
            CreateIndex("dbo.Shippings", "CityFrom_CityId");
            CreateIndex("dbo.Discounts", "Distance_DistanceId");
            AddForeignKey("dbo.Shippings", "CityTo_CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Shippings", "CityFrom_CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Discounts", "Distance_DistanceId", "dbo.Distances", "DistanceId");
        }
    }
}
