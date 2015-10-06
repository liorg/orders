namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "ActualStartDate", c => c.DateTime());
            AddColumn("dbo.Shippings", "ActualEndDate", c => c.DateTime());
            DropColumn("dbo.PriceLists", "SigBackType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceLists", "SigBackType", c => c.Int(nullable: false));
            DropColumn("dbo.Shippings", "ActualEndDate");
            DropColumn("dbo.Shippings", "ActualStartDate");
        }
    }
}
