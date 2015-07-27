namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "NotifyType", c => c.Int(nullable: false));
            AddColumn("dbo.Shippings", "NotifyText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "NotifyText");
            DropColumn("dbo.Shippings", "NotifyType");
        }
    }
}
