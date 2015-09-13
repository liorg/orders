namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class address : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "Target_CityCode", c => c.String());
            AddColumn("dbo.Shippings", "Target_CityName", c => c.String());
            AddColumn("dbo.Shippings", "Target_StreetCode", c => c.String());
            AddColumn("dbo.Shippings", "Target_StreetName", c => c.String());
            AddColumn("dbo.Shippings", "Target_ExtraDetail", c => c.String());
            AddColumn("dbo.Shippings", "Target_StreetNum", c => c.String());
            AddColumn("dbo.Shippings", "Target_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shippings", "Target_UID", c => c.Int(nullable: false));
            AddColumn("dbo.Shippings", "Target_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Shippings", "Target_Lng", c => c.Double(nullable: false));
            AddColumn("dbo.Shippings", "Source_CityCode", c => c.String());
            AddColumn("dbo.Shippings", "Source_CityName", c => c.String());
            AddColumn("dbo.Shippings", "Source_StreetCode", c => c.String());
            AddColumn("dbo.Shippings", "Source_StreetName", c => c.String());
            AddColumn("dbo.Shippings", "Source_ExtraDetail", c => c.String());
            AddColumn("dbo.Shippings", "Source_StreetNum", c => c.String());
            AddColumn("dbo.Shippings", "Source_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shippings", "Source_UID", c => c.Int(nullable: false));
            AddColumn("dbo.Shippings", "Source_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Shippings", "Source_Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "Source_Lng");
            DropColumn("dbo.Shippings", "Source_Lat");
            DropColumn("dbo.Shippings", "Source_UID");
            DropColumn("dbo.Shippings", "Source_IsSensor");
            DropColumn("dbo.Shippings", "Source_StreetNum");
            DropColumn("dbo.Shippings", "Source_ExtraDetail");
            DropColumn("dbo.Shippings", "Source_StreetName");
            DropColumn("dbo.Shippings", "Source_StreetCode");
            DropColumn("dbo.Shippings", "Source_CityName");
            DropColumn("dbo.Shippings", "Source_CityCode");
            DropColumn("dbo.Shippings", "Target_Lng");
            DropColumn("dbo.Shippings", "Target_Lat");
            DropColumn("dbo.Shippings", "Target_UID");
            DropColumn("dbo.Shippings", "Target_IsSensor");
            DropColumn("dbo.Shippings", "Target_StreetNum");
            DropColumn("dbo.Shippings", "Target_ExtraDetail");
            DropColumn("dbo.Shippings", "Target_StreetName");
            DropColumn("dbo.Shippings", "Target_StreetCode");
            DropColumn("dbo.Shippings", "Target_CityName");
            DropColumn("dbo.Shippings", "Target_CityCode");
        }
    }
}
