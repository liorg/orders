namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userre1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean());
        }
    }
}
