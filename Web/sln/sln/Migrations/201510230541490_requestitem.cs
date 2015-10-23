
namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestItemShips", "ObjectTypeId", c => c.Guid());
            AddColumn("dbo.RequestItemShips", "ObjectTypeIdCode", c => c.Int());
        }
        
        public override void Down()
        { 
            DropColumn("dbo.RequestItemShips", "ObjectTypeIdCode");
            DropColumn("dbo.RequestItemShips", "ObjectTypeId");
        }
    }
}
  