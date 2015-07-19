namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zzz : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrganizationDistances", newName: "DistanceOrganizations");
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountId = c.Guid(nullable: false),
                        Present = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinQuantity = c.Int(),
                        MaxQuantity = c.Int(),
                        Name = c.String(),
                        Precent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        PriceList_PriceListId = c.Guid(),
                    })
                .PrimaryKey(t => t.DiscountId)
                .ForeignKey("dbo.PriceLists", t => t.PriceList_PriceListId)
                .Index(t => t.PriceList_PriceListId);
            
            AddColumn("dbo.PriceLists", "MinTimeWait", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Discounts", "PriceList_PriceListId", "dbo.PriceLists");
            DropIndex("dbo.Discounts", new[] { "PriceList_PriceListId" });
            DropColumn("dbo.PriceLists", "MinTimeWait");
            DropTable("dbo.Discounts");
            RenameTable(name: "dbo.DistanceOrganizations", newName: "OrganizationDistances");
        }
    }
}
