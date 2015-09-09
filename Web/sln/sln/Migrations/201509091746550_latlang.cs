namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latlang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableTests", "ShippingAddress_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "ShippingAddress_Lng", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableTests", "BillingAddress_Lng");
            DropColumn("dbo.TableTests", "BillingAddress_Lat");
            DropColumn("dbo.TableTests", "ShippingAddress_Lng");
            DropColumn("dbo.TableTests", "ShippingAddress_Lat");
        }
    }
}
