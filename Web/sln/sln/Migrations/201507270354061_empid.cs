namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class empid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmpId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EmpId");
        }
    }
}
