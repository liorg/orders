namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descadd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Distances", "Desc", c => c.String());
            AddColumn("dbo.Products", "Desc", c => c.String());
            AddColumn("dbo.ShipTypes", "Desc", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipTypes", "Desc");
            DropColumn("dbo.Products", "Desc");
            DropColumn("dbo.Distances", "Desc");
        }
    }
}
