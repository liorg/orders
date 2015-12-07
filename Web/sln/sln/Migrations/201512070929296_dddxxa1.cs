namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dddxxa1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "DistanceValue", c => c.Single(nullable: false));
            AddColumn("dbo.Shippings", "DistanceText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "DistanceText");
            DropColumn("dbo.Shippings", "DistanceValue");
        }
    }
}
