namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DistanceOrganizations", newName: "OrganizationDistances");
            CreateTable(
                "dbo.PriceListForOrgs",
                c => new
                    {
                        PriceListForOrgId = c.Guid(nullable: false),
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
                        Organizations_OrgId = c.Guid(),
                    })
                .PrimaryKey(t => t.PriceListForOrgId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .Index(t => t.Organizations_OrgId);
            
            CreateTable(
                "dbo.Shippings",
                c => new
                    {
                        ShippingId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        AddressFrom = c.String(),
                        AddressTo = c.String(),
                        AddressNumFrom = c.String(),
                        AddressNumTo = c.String(),
                        City_CityId = c.Guid(),
                        City_CityId1 = c.Guid(),
                        CityFrom_CityId = c.Guid(),
                        CityTo_CityId = c.Guid(),
                        Distance_DistanceId = c.Guid(),
                        Organization_OrgId = c.Guid(),
                        StatusShipping_StatusShippingId = c.Guid(),
                    })
                .PrimaryKey(t => t.ShippingId)
                .ForeignKey("dbo.Cities", t => t.City_CityId)
                .ForeignKey("dbo.Cities", t => t.City_CityId1)
                .ForeignKey("dbo.Cities", t => t.CityFrom_CityId)
                .ForeignKey("dbo.Cities", t => t.CityTo_CityId)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId)
                .ForeignKey("dbo.StatusShippings", t => t.StatusShipping_StatusShippingId)
                .Index(t => t.City_CityId)
                .Index(t => t.City_CityId1)
                .Index(t => t.CityFrom_CityId)
                .Index(t => t.CityTo_CityId)
                .Index(t => t.Distance_DistanceId)
                .Index(t => t.Organization_OrgId)
                .Index(t => t.StatusShipping_StatusShippingId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId);
            
            CreateTable(
                "dbo.ShippingItems",
                c => new
                    {
                        ShippingItemId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                    })
                .PrimaryKey(t => t.ShippingItemId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .Index(t => t.Shipping_ShippingId);
            
            CreateTable(
                "dbo.StatusShippings",
                c => new
                    {
                        StatusShippingId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StatusShippingId);
            
            CreateTable(
                "dbo.TimeLines",
                c => new
                    {
                        TimeLineId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        DescHtml = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                        StatusTimeLine_StatusTimeLineId = c.Guid(),
                    })
                .PrimaryKey(t => t.TimeLineId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .ForeignKey("dbo.StatusTimeLines", t => t.StatusTimeLine_StatusTimeLineId)
                .Index(t => t.Shipping_ShippingId)
                .Index(t => t.StatusTimeLine_StatusTimeLineId);
            
            CreateTable(
                "dbo.StatusTimeLines",
                c => new
                    {
                        StatusTimeLineId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StatusTimeLineId);
            
            CreateTable(
                "dbo.PriceCalcs",
                c => new
                    {
                        PriceCalcId = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        ShippingId = c.Guid(nullable: false),
                        ShippingItemId = c.Guid(nullable: false),
                        PriceListId = c.Guid(nullable: false),
                        DiscountId = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Present = c.Decimal(precision: 18, scale: 2),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PriceCalcId);
            
            AddColumn("dbo.Discounts", "Distance_DistanceId", c => c.Guid());
            AddColumn("dbo.PriceLists", "Distance_DistanceId", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean());
            AddColumn("dbo.AspNetUsers", "Department", c => c.String());
            AddColumn("dbo.AspNetUsers", "Subdivision", c => c.String());
            CreateIndex("dbo.Discounts", "Distance_DistanceId");
            CreateIndex("dbo.PriceLists", "Distance_DistanceId");
            AddForeignKey("dbo.Discounts", "Distance_DistanceId", "dbo.Distances", "DistanceId");
            AddForeignKey("dbo.PriceLists", "Distance_DistanceId", "dbo.Distances", "DistanceId");
            DropColumn("dbo.PriceLists", "MinTimeWait");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceLists", "MinTimeWait", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.TimeLines", "StatusTimeLine_StatusTimeLineId", "dbo.StatusTimeLines");
            DropForeignKey("dbo.TimeLines", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.Shippings", "StatusShipping_StatusShippingId", "dbo.StatusShippings");
            DropForeignKey("dbo.ShippingItems", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.Shippings", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Shippings", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Shippings", "CityTo_CityId", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "CityFrom_CityId", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "City_CityId1", "dbo.Cities");
            DropForeignKey("dbo.Shippings", "City_CityId", "dbo.Cities");
            DropForeignKey("dbo.PriceListForOrgs", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.PriceLists", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Discounts", "Distance_DistanceId", "dbo.Distances");
            DropIndex("dbo.TimeLines", new[] { "StatusTimeLine_StatusTimeLineId" });
            DropIndex("dbo.TimeLines", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.Shippings", new[] { "StatusShipping_StatusShippingId" });
            DropIndex("dbo.ShippingItems", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.Shippings", new[] { "Organization_OrgId" });
            DropIndex("dbo.Shippings", new[] { "Distance_DistanceId" });
            DropIndex("dbo.Shippings", new[] { "CityTo_CityId" });
            DropIndex("dbo.Shippings", new[] { "CityFrom_CityId" });
            DropIndex("dbo.Shippings", new[] { "City_CityId1" });
            DropIndex("dbo.Shippings", new[] { "City_CityId" });
            DropIndex("dbo.PriceListForOrgs", new[] { "Organizations_OrgId" });
            DropIndex("dbo.PriceLists", new[] { "Distance_DistanceId" });
            DropIndex("dbo.Discounts", new[] { "Distance_DistanceId" });
            DropColumn("dbo.AspNetUsers", "Subdivision");
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "IsActive");
            DropColumn("dbo.PriceLists", "Distance_DistanceId");
            DropColumn("dbo.Discounts", "Distance_DistanceId");
            DropTable("dbo.PriceCalcs");
            DropTable("dbo.StatusTimeLines");
            DropTable("dbo.TimeLines");
            DropTable("dbo.StatusShippings");
            DropTable("dbo.ShippingItems");
            DropTable("dbo.Cities");
            DropTable("dbo.Shippings");
            DropTable("dbo.PriceListForOrgs");
            RenameTable(name: "dbo.OrganizationDistances", newName: "DistanceOrganizations");
        }
    }
}
