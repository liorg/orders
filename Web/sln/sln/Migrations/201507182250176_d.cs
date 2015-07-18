namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Organizations", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Organizations", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Organizations", "ModifiedBy", c => c.Guid());
            AddColumn("dbo.Organizations", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "IsActive");
            DropColumn("dbo.Organizations", "ModifiedBy");
            DropColumn("dbo.Organizations", "CreatedBy");
            DropColumn("dbo.Organizations", "ModifiedOn");
            DropColumn("dbo.Organizations", "CreatedOn");
        }
    }
}
