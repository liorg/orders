namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttachmentShippings", "Path", c => c.String());
            AddColumn("dbo.AttachmentShippings", "IsSign", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentShippings", "IsSign");
            DropColumn("dbo.AttachmentShippings", "Path");
        }
    }
}
