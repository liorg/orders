namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "ApprovalRequest", c => c.Guid());
            AddColumn("dbo.Shippings", "ApprovalShip", c => c.Guid());
            AddColumn("dbo.TimeLines", "ApprovalRequest", c => c.Guid());
            AddColumn("dbo.TimeLines", "ApprovalShip", c => c.Guid());
            AddColumn("dbo.TimeLines", "OwnerShip", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeLines", "OwnerShip");
            DropColumn("dbo.TimeLines", "ApprovalShip");
            DropColumn("dbo.TimeLines", "ApprovalRequest");
            DropColumn("dbo.Shippings", "ApprovalShip");
            DropColumn("dbo.Shippings", "ApprovalRequest");
        }
    }
}
