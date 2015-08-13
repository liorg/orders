namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gggg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "ActualRecipient", c => c.String());
            AddColumn("dbo.Shippings", "TelSource", c => c.String());
            AddColumn("dbo.Shippings", "TelTarget", c => c.String());
            AddColumn("dbo.Shippings", "NameSource", c => c.String());
            AddColumn("dbo.Shippings", "NameTarget", c => c.String());
            AddColumn("dbo.AspNetUsers", "Tel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Tel");
            DropColumn("dbo.Shippings", "NameTarget");
            DropColumn("dbo.Shippings", "NameSource");
            DropColumn("dbo.Shippings", "TelTarget");
            DropColumn("dbo.Shippings", "TelSource");
            DropColumn("dbo.Shippings", "ActualRecipient");
        }
    }
}
