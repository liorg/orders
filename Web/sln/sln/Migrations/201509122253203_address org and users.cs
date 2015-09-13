namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addressorgandusers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "AddressOrg_CityCode", c => c.String());
            AddColumn("dbo.Organizations", "AddressOrg_CityName", c => c.String());
            AddColumn("dbo.Organizations", "AddressOrg_StreetCode", c => c.String());
            AddColumn("dbo.Organizations", "AddressOrg_StreetName", c => c.String());
            AddColumn("dbo.Organizations", "AddressOrg_ExtraDetail", c => c.String());
            AddColumn("dbo.Organizations", "AddressOrg_StreetNum", c => c.String());
            AddColumn("dbo.Organizations", "AddressOrg_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "AddressOrg_UID", c => c.Int(nullable: false));
            AddColumn("dbo.Organizations", "AddressOrg_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Organizations", "AddressOrg_Lng", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "AddressUser_CityCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressUser_CityName", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressUser_StreetCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressUser_StreetName", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressUser_ExtraDetail", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressUser_StreetNum", c => c.String());
            AddColumn("dbo.AspNetUsers", "AddressUser_IsSensor", c => c.Boolean());
            AddColumn("dbo.AspNetUsers", "AddressUser_UID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "AddressUser_Lat", c => c.Double());
            AddColumn("dbo.AspNetUsers", "AddressUser_Lng", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AddressUser_Lng");
            DropColumn("dbo.AspNetUsers", "AddressUser_Lat");
            DropColumn("dbo.AspNetUsers", "AddressUser_UID");
            DropColumn("dbo.AspNetUsers", "AddressUser_IsSensor");
            DropColumn("dbo.AspNetUsers", "AddressUser_StreetNum");
            DropColumn("dbo.AspNetUsers", "AddressUser_ExtraDetail");
            DropColumn("dbo.AspNetUsers", "AddressUser_StreetName");
            DropColumn("dbo.AspNetUsers", "AddressUser_StreetCode");
            DropColumn("dbo.AspNetUsers", "AddressUser_CityName");
            DropColumn("dbo.AspNetUsers", "AddressUser_CityCode");
            DropColumn("dbo.Organizations", "AddressOrg_Lng");
            DropColumn("dbo.Organizations", "AddressOrg_Lat");
            DropColumn("dbo.Organizations", "AddressOrg_UID");
            DropColumn("dbo.Organizations", "AddressOrg_IsSensor");
            DropColumn("dbo.Organizations", "AddressOrg_StreetNum");
            DropColumn("dbo.Organizations", "AddressOrg_ExtraDetail");
            DropColumn("dbo.Organizations", "AddressOrg_StreetName");
            DropColumn("dbo.Organizations", "AddressOrg_StreetCode");
            DropColumn("dbo.Organizations", "AddressOrg_CityName");
            DropColumn("dbo.Organizations", "AddressOrg_CityCode");
        }
    }
}
