namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "GrantRunner", c => c.Guid());
            AddColumn("dbo.Shippings", "Runner", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "Runner");
            DropColumn("dbo.Shippings", "GrantRunner");
        }
    }
}
