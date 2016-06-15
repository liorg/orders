namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ParentProductId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ParentProductId");
        }
    }
}
