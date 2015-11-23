namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userapprovalforship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "ApprovalPriceException", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "ApprovalPriceException");
        }
    }
}
