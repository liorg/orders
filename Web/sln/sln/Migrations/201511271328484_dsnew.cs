namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dsnew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "IsInProccess", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShippingCompanies", "AddressCompany_CityCode", c => c.String());
            AddColumn("dbo.ShippingCompanies", "AddressCompany_CityName", c => c.String());
            AddColumn("dbo.ShippingCompanies", "AddressCompany_StreetCode", c => c.String());
            AddColumn("dbo.ShippingCompanies", "AddressCompany_StreetName", c => c.String());
            AddColumn("dbo.ShippingCompanies", "AddressCompany_ExtraDetail", c => c.String());
            AddColumn("dbo.ShippingCompanies", "AddressCompany_StreetNum", c => c.String());
            AddColumn("dbo.ShippingCompanies", "AddressCompany_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShippingCompanies", "AddressCompany_UID", c => c.Int(nullable: false));
            AddColumn("dbo.ShippingCompanies", "AddressCompany_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.ShippingCompanies", "AddressCompany_Lng", c => c.Double(nullable: false));
            AddColumn("dbo.ShippingCompanies", "Tel", c => c.String());
            AddColumn("dbo.ShippingCompanies", "ContactFullName", c => c.String());
            AddColumn("dbo.ShippingCompanies", "ContactTel", c => c.String());
            DropColumn("dbo.Slas", "IsBusinessDay");
            DropColumn("dbo.Slas", "Days");
            DropColumn("dbo.Slas", "Hours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Slas", "Hours", c => c.Double(nullable: false));
            AddColumn("dbo.Slas", "Days", c => c.Double(nullable: false));
            AddColumn("dbo.Slas", "IsBusinessDay", c => c.Boolean(nullable: false));
            DropColumn("dbo.ShippingCompanies", "ContactTel");
            DropColumn("dbo.ShippingCompanies", "ContactFullName");
            DropColumn("dbo.ShippingCompanies", "Tel");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_Lng");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_Lat");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_UID");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_IsSensor");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_StreetNum");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_ExtraDetail");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_StreetName");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_StreetCode");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_CityName");
            DropColumn("dbo.ShippingCompanies", "AddressCompany_CityCode");
            DropColumn("dbo.Shippings", "IsInProccess");
        }
    }
}
