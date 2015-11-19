namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requessted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestItemShips", "ProductValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BussinessClosures", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.RequestItemShips", "StatusCode");
            DropColumn("dbo.RequestItemShips", "ReqeustType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestItemShips", "ReqeustType", c => c.Int(nullable: false));
            AddColumn("dbo.RequestItemShips", "StatusCode", c => c.Int(nullable: false));
            DropColumn("dbo.BussinessClosures", "Year");
            DropColumn("dbo.RequestItemShips", "ProductValue");
        }
    }
}
