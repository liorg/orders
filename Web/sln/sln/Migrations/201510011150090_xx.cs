namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xx : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "CloseDesc", c => c.String());
            AddColumn("dbo.Shippings", "SigBackType", c => c.Int());
            AlterColumn("dbo.Shippings", "SlaTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shippings", "SlaTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Shippings", "SigBackType");
            DropColumn("dbo.Shippings", "CloseDesc");
        }
    }
}
