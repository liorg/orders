namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestship33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceLists", "PriceValueType", c => c.Int(nullable: false));
            DropColumn("dbo.PriceLists", "PriceType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceLists", "PriceType", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.PriceLists", "PriceValueType");
        }
    }
}
