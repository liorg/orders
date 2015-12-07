namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dddxxa134 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "FixedDistanceValue", c => c.Double(nullable: false));
            AddColumn("dbo.DistanceCities", "FixedDistanceValue", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistanceCities", "FixedDistanceValue");
            DropColumn("dbo.Shippings", "FixedDistanceValue");
        }
    }
}
