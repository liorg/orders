namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grantg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "BroughtShipping", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "DefaultView", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ViewAll", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ViewAll");
            DropColumn("dbo.AspNetUsers", "DefaultView");
            DropColumn("dbo.Shippings", "BroughtShipping");
        }
    }
}
