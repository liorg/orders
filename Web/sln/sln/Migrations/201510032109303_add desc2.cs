namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddesc2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceLists", "SigBackType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceLists", "SigBackType");
        }
    }
}
