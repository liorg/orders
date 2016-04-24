namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addextrafields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "ShippingCompanyIdDefault", c => c.Guid());
            AddColumn("dbo.MoreAddress", "Tel1", c => c.String());
            AddColumn("dbo.MoreAddress", "Name1", c => c.String());
            AddColumn("dbo.MoreAddress", "Name2", c => c.String());
            AddColumn("dbo.TableTests", "Code", c => c.Int(nullable: false));
            AddColumn("dbo.TableTests", "Name", c => c.String());
            AlterColumn("dbo.Organizations", "OrganizationCode", c => c.Int(nullable: false));
            DropColumn("dbo.TableTests", "ShippingAddress_CityCode");
            DropColumn("dbo.TableTests", "ShippingAddress_CityName");
            DropColumn("dbo.TableTests", "ShippingAddress_StreetCode");
            DropColumn("dbo.TableTests", "ShippingAddress_StreetName");
            DropColumn("dbo.TableTests", "ShippingAddress_ExtraDetail");
            DropColumn("dbo.TableTests", "ShippingAddress_StreetNum");
            DropColumn("dbo.TableTests", "ShippingAddress_IsSensor");
            DropColumn("dbo.TableTests", "ShippingAddress_UID");
            DropColumn("dbo.TableTests", "ShippingAddress_Lat");
            DropColumn("dbo.TableTests", "ShippingAddress_Lng");
            DropColumn("dbo.TableTests", "BillingAddress_CityCode");
            DropColumn("dbo.TableTests", "BillingAddress_CityName");
            DropColumn("dbo.TableTests", "BillingAddress_StreetCode");
            DropColumn("dbo.TableTests", "BillingAddress_StreetName");
            DropColumn("dbo.TableTests", "BillingAddress_ExtraDetail");
            DropColumn("dbo.TableTests", "BillingAddress_StreetNum");
            DropColumn("dbo.TableTests", "BillingAddress_IsSensor");
            DropColumn("dbo.TableTests", "BillingAddress_UID");
            DropColumn("dbo.TableTests", "BillingAddress_Lat");
            DropColumn("dbo.TableTests", "BillingAddress_Lng");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TableTests", "BillingAddress_Lng", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_UID", c => c.Int(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_StreetNum", c => c.String());
            AddColumn("dbo.TableTests", "BillingAddress_ExtraDetail", c => c.String());
            AddColumn("dbo.TableTests", "BillingAddress_StreetName", c => c.String());
            AddColumn("dbo.TableTests", "BillingAddress_StreetCode", c => c.String());
            AddColumn("dbo.TableTests", "BillingAddress_CityName", c => c.String());
            AddColumn("dbo.TableTests", "BillingAddress_CityCode", c => c.String());
            AddColumn("dbo.TableTests", "ShippingAddress_Lng", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "ShippingAddress_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "ShippingAddress_UID", c => c.Int(nullable: false));
            AddColumn("dbo.TableTests", "ShippingAddress_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.TableTests", "ShippingAddress_StreetNum", c => c.String());
            AddColumn("dbo.TableTests", "ShippingAddress_ExtraDetail", c => c.String());
            AddColumn("dbo.TableTests", "ShippingAddress_StreetName", c => c.String());
            AddColumn("dbo.TableTests", "ShippingAddress_StreetCode", c => c.String());
            AddColumn("dbo.TableTests", "ShippingAddress_CityName", c => c.String());
            AddColumn("dbo.TableTests", "ShippingAddress_CityCode", c => c.String());
            AlterColumn("dbo.Organizations", "OrganizationCode", c => c.Int());
            DropColumn("dbo.TableTests", "Name");
            DropColumn("dbo.TableTests", "Code");
            DropColumn("dbo.MoreAddress", "Name2");
            DropColumn("dbo.MoreAddress", "Name1");
            DropColumn("dbo.MoreAddress", "Tel1");
            DropColumn("dbo.Organizations", "ShippingCompanyIdDefault");
        }
    }
}
