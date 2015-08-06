namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricecalc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceCalcs", "DistanceId", c => c.Guid(nullable: false));
            AddColumn("dbo.PriceCalcs", "ShipType", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceCalcs", "ShipType");
            DropColumn("dbo.PriceCalcs", "DistanceId");
        }
    }
}
