namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricee1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PriceListForOrgs", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Discounts", "Distance_DistanceId1", "dbo.Distances");
            DropIndex("dbo.PriceListForOrgs", new[] { "Organizations_OrgId" });
            DropIndex("dbo.Discounts", new[] { "Distance_DistanceId1" });
            AddColumn("dbo.Discounts", "MinTimeWait", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Discounts", "DecreasePriceFixed", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.Products", "IsCalculatingShippingInclusive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shippings", "OwnerId", c => c.Guid());
            CreateIndex("dbo.Discounts", "Distance_DistanceId");
            AddForeignKey("dbo.Discounts", "Distance_DistanceId", "dbo.Distances", "DistanceId");
            DropColumn("dbo.Discounts", "Distance_DistanceId1");
            DropTable("dbo.PriceListForOrgs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PriceListForOrgs",
                c => new
                    {
                        PriceListForOrgId = c.Guid(nullable: false),
                        Organizations_OrgId = c.Guid(),
                        Name = c.String(),
                        MinTimeWait = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Present = c.Decimal(precision: 18, scale: 2),
                        Desc = c.String(),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PriceListForOrgId);
            
            AddColumn("dbo.Discounts", "Distance_DistanceId1", c => c.Guid());
            DropForeignKey("dbo.Discounts", "Distance_DistanceId", "dbo.Distances");
            DropIndex("dbo.Discounts", new[] { "Distance_DistanceId" });
            DropColumn("dbo.Shippings", "OwnerId");
            DropColumn("dbo.Products", "IsCalculatingShippingInclusive");
            DropColumn("dbo.Discounts", "DecreasePriceFixed");
            DropColumn("dbo.Discounts", "MinTimeWait");
            CreateIndex("dbo.Discounts", "Distance_DistanceId1");
            CreateIndex("dbo.PriceListForOrgs", "Organizations_OrgId");
            AddForeignKey("dbo.Discounts", "Distance_DistanceId1", "dbo.Distances", "DistanceId");
            AddForeignKey("dbo.PriceListForOrgs", "Organizations_OrgId", "dbo.Organizations", "OrgId");
        }
    }
}
