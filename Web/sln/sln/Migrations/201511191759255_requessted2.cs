namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requessted2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestItemShips", "IsDiscount", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestItemShips", "QuntityType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestItemShips", "QuntityType");
            DropColumn("dbo.RequestItemShips", "IsDiscount");
        }
    }
}
