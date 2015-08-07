namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ggggr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "NoBroughtShipmentCustomer", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "NoBroughtShipmentCustomer");
        }
    }
}
