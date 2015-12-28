namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestItemShips", "StatusRecord", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestItemShips", "StatusRecord");
        }
    }
}
