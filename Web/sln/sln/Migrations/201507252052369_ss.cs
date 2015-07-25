namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeLines", "StatusShipping_StatusShippingId", c => c.Guid());
            CreateIndex("dbo.TimeLines", "StatusShipping_StatusShippingId");
            AddForeignKey("dbo.TimeLines", "StatusShipping_StatusShippingId", "dbo.StatusShippings", "StatusShippingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeLines", "StatusShipping_StatusShippingId", "dbo.StatusShippings");
            DropIndex("dbo.TimeLines", new[] { "StatusShipping_StatusShippingId" });
            DropColumn("dbo.TimeLines", "StatusShipping_StatusShippingId");
        }
    }
}
