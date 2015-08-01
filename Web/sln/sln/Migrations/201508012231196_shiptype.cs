namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shiptype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShipTypes",
                c => new
                    {
                        ShipTypeId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShipTypeId);
            
            AddColumn("dbo.Discounts", "ShipType_ShipTypeId", c => c.Guid());
            AddColumn("dbo.PriceLists", "ShipType_ShipTypeId", c => c.Guid());
            AddColumn("dbo.Shippings", "ShipType_ShipTypeId", c => c.Guid());
            CreateIndex("dbo.Discounts", "ShipType_ShipTypeId");
            CreateIndex("dbo.Shippings", "ShipType_ShipTypeId");
            CreateIndex("dbo.PriceLists", "ShipType_ShipTypeId");
            AddForeignKey("dbo.Discounts", "ShipType_ShipTypeId", "dbo.ShipTypes", "ShipTypeId");
            AddForeignKey("dbo.Shippings", "ShipType_ShipTypeId", "dbo.ShipTypes", "ShipTypeId");
            AddForeignKey("dbo.PriceLists", "ShipType_ShipTypeId", "dbo.ShipTypes", "ShipTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceLists", "ShipType_ShipTypeId", "dbo.ShipTypes");
            DropForeignKey("dbo.Shippings", "ShipType_ShipTypeId", "dbo.ShipTypes");
            DropForeignKey("dbo.Discounts", "ShipType_ShipTypeId", "dbo.ShipTypes");
            DropIndex("dbo.PriceLists", new[] { "ShipType_ShipTypeId" });
            DropIndex("dbo.Shippings", new[] { "ShipType_ShipTypeId" });
            DropIndex("dbo.Discounts", new[] { "ShipType_ShipTypeId" });
            DropColumn("dbo.Shippings", "ShipType_ShipTypeId");
            DropColumn("dbo.PriceLists", "ShipType_ShipTypeId");
            DropColumn("dbo.Discounts", "ShipType_ShipTypeId");
            DropTable("dbo.ShipTypes");
        }
    }
}
