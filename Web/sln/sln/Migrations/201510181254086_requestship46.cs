namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestship46 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PriceLists", "PriceValue", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PriceLists", "PriceValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
