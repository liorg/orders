namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StatusShippings", "OrderDirection", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StatusShippings", "OrderDirection");
        }
    }
}
