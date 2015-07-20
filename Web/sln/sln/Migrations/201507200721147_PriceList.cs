namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discounts", "MinQuantityOneWay", c => c.Int());
            AddColumn("dbo.PriceCalcs", "Total", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "TotalPerPruduct", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "IncrementalItem", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "IncrementalPerPruduct", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "PruductId", c => c.Guid(nullable: false));
            DropColumn("dbo.Discounts", "MinQuantity");
            DropColumn("dbo.Discounts", "MaxQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Discounts", "MaxQuantity", c => c.Int());
            AddColumn("dbo.Discounts", "MinQuantity", c => c.Int());
            DropColumn("dbo.PriceCalcs", "PruductId");
            DropColumn("dbo.PriceCalcs", "IncrementalPerPruduct");
            DropColumn("dbo.PriceCalcs", "IncrementalItem");
            DropColumn("dbo.PriceCalcs", "TotalPerPruduct");
            DropColumn("dbo.PriceCalcs", "Total");
            DropColumn("dbo.Discounts", "MinQuantityOneWay");
        }
    }
}
