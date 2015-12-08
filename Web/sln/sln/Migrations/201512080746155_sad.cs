namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistanceCities", "CityName1", c => c.String());
            AddColumn("dbo.DistanceCities", "CityName2", c => c.String());
            AddColumn("dbo.DistanceCities", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistanceCities", "Url");
            DropColumn("dbo.DistanceCities", "CityName2");
            DropColumn("dbo.DistanceCities", "CityName1");
        }
    }
}
