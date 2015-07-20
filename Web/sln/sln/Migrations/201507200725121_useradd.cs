namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useradd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsActive");
        }
    }
}
