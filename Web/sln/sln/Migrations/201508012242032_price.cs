namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discounts", "QuantityType", c => c.Int(nullable: false));
            AddColumn("dbo.Discounts", "MinQuantity", c => c.Int());
            AddColumn("dbo.Discounts", "MaxQuantity", c => c.Int());
            AddColumn("dbo.Discounts", "PriceValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Discounts", "PriceValueType", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "TotalPerProduct", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "IncrementalPerProduct", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "PriceCap", c => c.Decimal(nullable: false, storeType: "money"));
            DropColumn("dbo.Discounts", "Present");
            DropColumn("dbo.Discounts", "MinQuantityOneWay");
            DropColumn("dbo.Discounts", "MinTimeWait");
            DropColumn("dbo.Discounts", "DecreasePriceFixed");
            DropColumn("dbo.Discounts", "Precent");
            DropColumn("dbo.PriceCalcs", "TotalPerPruduct");
            DropColumn("dbo.PriceCalcs", "IncrementalPerPruduct");
            DropColumn("dbo.PriceCalcs", "Present");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceCalcs", "Present", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PriceCalcs", "IncrementalPerPruduct", c => c.Int(nullable: false));
            AddColumn("dbo.PriceCalcs", "TotalPerPruduct", c => c.Int(nullable: false));
            AddColumn("dbo.Discounts", "Precent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Discounts", "DecreasePriceFixed", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.Discounts", "MinTimeWait", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Discounts", "MinQuantityOneWay", c => c.Int());
            AddColumn("dbo.Discounts", "Present", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.PriceCalcs", "PriceCap");
            DropColumn("dbo.PriceCalcs", "IncrementalPerProduct");
            DropColumn("dbo.PriceCalcs", "TotalPerProduct");
            DropColumn("dbo.Discounts", "PriceValueType");
            DropColumn("dbo.Discounts", "PriceValue");
            DropColumn("dbo.Discounts", "MaxQuantity");
            DropColumn("dbo.Discounts", "MinQuantity");
            DropColumn("dbo.Discounts", "QuantityType");
        }
    }
}
