namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fastsearch2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "FastSearchNumber", c => c.Long(nullable: false));
            DropColumn("dbo.Shippings", "FastSearch");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shippings", "FastSearch", c => c.Int(nullable: false));
            DropColumn("dbo.Shippings", "FastSearchNumber");
        }
    }
}
