namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableTests", "ShippingAddress_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.TableTests", "ShippingAddress_UID", c => c.Int(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_IsSensor", c => c.Boolean(nullable: false));
            AddColumn("dbo.TableTests", "BillingAddress_UID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableTests", "BillingAddress_UID");
            DropColumn("dbo.TableTests", "BillingAddress_IsSensor");
            DropColumn("dbo.TableTests", "ShippingAddress_UID");
            DropColumn("dbo.TableTests", "ShippingAddress_IsSensor");
        }
    }
}
