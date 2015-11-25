namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bsdays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BussinessClosures", "ShippingCompany", c => c.Guid(nullable: false));
            AlterColumn("dbo.BussinessClosures", "SpecialDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BussinessClosures", "SpecialDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.BussinessClosures", "ShippingCompany");
        }
    }
}
