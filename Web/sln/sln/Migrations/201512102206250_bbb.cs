namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bbb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippings", "ArrivedShippingGet", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shippings", "ArrivedShippingGet");
        }
    }
}
