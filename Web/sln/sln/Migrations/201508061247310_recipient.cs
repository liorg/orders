namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "Recipient", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "Recipient");
        }
    }
}
