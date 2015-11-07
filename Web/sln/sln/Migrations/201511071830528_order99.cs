namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order99 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "OfferId", c => c.Guid());
            AddColumn("dbo.PriceLists", "QuntityType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceLists", "QuntityType");
            DropColumn("dbo.Shippings", "OfferId");
        }
    }
}
