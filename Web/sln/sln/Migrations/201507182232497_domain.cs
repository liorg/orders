namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class domain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "Domain", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "Domain");
        }
    }
}
