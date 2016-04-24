namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addextrafields2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingCompanies", "Perfix", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShippingCompanies", "Perfix");
        }
    }
}
