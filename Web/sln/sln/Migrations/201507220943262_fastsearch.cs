namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fastsearch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "FastSearch", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "FastSearch");
        }
    }
}
