namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttachmentShippings",
                c => new
                    {
                        CommentId = c.Guid(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                        Name = c.String(),
                        Path = c.String(),
                        IsSign = c.Boolean(nullable: false),
                        TypeMime = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .Index(t => t.Shipping_ShippingId);
            
            CreateTable(
                "dbo.Shippings",
                c => new
                    {
                        ShippingId = c.Guid(nullable: false),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        Target_CityCode = c.String(),
                        Target_CityName = c.String(),
                        Target_StreetCode = c.String(),
                        Target_StreetName = c.String(),
                        Target_ExtraDetail = c.String(),
                        Target_StreetNum = c.String(),
                        Target_IsSensor = c.Boolean(nullable: false),
                        Target_UID = c.Int(nullable: false),
                        Target_Lat = c.Double(nullable: false),
                        Target_Lng = c.Double(nullable: false),
                        Source_CityCode = c.String(),
                        Source_CityName = c.String(),
                        Source_StreetCode = c.String(),
                        Source_StreetName = c.String(),
                        Source_ExtraDetail = c.String(),
                        Source_StreetNum = c.String(),
                        Source_IsSensor = c.Boolean(nullable: false),
                        Source_UID = c.Int(nullable: false),
                        Source_Lat = c.Double(nullable: false),
                        Source_Lng = c.Double(nullable: false),
                        ShipType_ShipTypeId = c.Guid(),
                        Distance_DistanceId = c.Guid(),
                        StatusShipping_StatusShippingId = c.Guid(),
                        Organization_OrgId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        OwnerId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        DiscountPrice = c.Decimal(nullable: false, storeType: "money"),
                        ActualPrice = c.Decimal(nullable: false, storeType: "money"),
                        TimeWait = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimatedPrice = c.Decimal(nullable: false, storeType: "money"),
                        FastSearchNumber = c.Long(nullable: false),
                        ApprovalRequest = c.Guid(),
                        ApprovalShip = c.Guid(),
                        GrantRunner = c.Guid(),
                        Runner = c.Guid(),
                        BroughtShippingSender = c.Guid(),
                        BroughtShipmentCustomer = c.Guid(),
                        NoBroughtShipmentCustomer = c.Guid(),
                        CancelByUser = c.Guid(),
                        CancelByAdmin = c.Guid(),
                        ArrivedShippingSender = c.Guid(),
                        ClosedShippment = c.Guid(),
                        NotifyType = c.Int(nullable: false),
                        NotifyText = c.String(),
                        Recipient = c.String(),
                        ActualRecipient = c.String(),
                        TelSource = c.String(),
                        TelTarget = c.String(),
                        NameSource = c.String(),
                        NameTarget = c.String(),
                        SlaTime = c.DateTime(),
                        ActualTelTarget = c.String(),
                        ActualNameTarget = c.String(),
                        CloseDesc = c.String(),
                        EndDesc = c.String(),
                        SigBackType = c.Int(),
                        ActualStartDate = c.DateTime(),
                        ActualEndDate = c.DateTime(),
                        Direction = c.Int(nullable: false),
                        TimeWaitStartSend = c.DateTime(),
                        TimeWaitEndSend = c.DateTime(),
                        TimeWaitStartSGet = c.DateTime(),
                        TimeWaitEndGet = c.DateTime(),
                    })
                .PrimaryKey(t => t.ShippingId)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .ForeignKey("dbo.ShipTypes", t => t.ShipType_ShipTypeId)
                .ForeignKey("dbo.StatusShippings", t => t.StatusShipping_StatusShippingId)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.ShipType_ShipTypeId)
                .Index(t => t.Distance_DistanceId)
                .Index(t => t.StatusShipping_StatusShippingId)
                .Index(t => t.Organization_OrgId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Guid(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                        JobType = c.String(),
                        JobTitle = c.String(),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .Index(t => t.Shipping_ShippingId);
            
            CreateTable(
                "dbo.Distances",
                c => new
                    {
                        DistanceId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DistanceId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrgId = c.Guid(nullable: false),
                        AddressOrg_CityCode = c.String(),
                        AddressOrg_CityName = c.String(),
                        AddressOrg_StreetCode = c.String(),
                        AddressOrg_StreetName = c.String(),
                        AddressOrg_ExtraDetail = c.String(),
                        AddressOrg_StreetNum = c.String(),
                        AddressOrg_IsSensor = c.Boolean(nullable: false),
                        AddressOrg_UID = c.Int(nullable: false),
                        AddressOrg_Lat = c.Double(nullable: false),
                        AddressOrg_Lng = c.Double(nullable: false),
                        Name = c.String(),
                        Domain = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrgId);
            
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        PriceListId = c.Guid(nullable: false),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        Organizations_OrgId = c.Guid(),
                        ObjectId = c.Guid(nullable: false),
                        ObjectTypeCode = c.Int(nullable: false),
                        PriceValue = c.Decimal(precision: 18, scale: 2),
                        PriceValueType = c.Int(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PriceListId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Organizations_OrgId);
            
            CreateTable(
                "dbo.ShippingCompanies",
                c => new
                    {
                        ShippingCompanyId = c.Guid(nullable: false),
                        Name = c.String(),
                        Desc = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingCompanyId);
            
            CreateTable(
                "dbo.RequestShippings",
                c => new
                    {
                        RequestShippingId = c.Guid(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        Organizations_OrgId = c.Guid(),
                        StatusCode = c.Int(nullable: false),
                        StatusReasonCode = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        DiscountPrice = c.Decimal(nullable: false, storeType: "money"),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        OwnerId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestShippingId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Shipping_ShippingId)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Organizations_OrgId);
            
            CreateTable(
                "dbo.RequestItemShips",
                c => new
                    {
                        RequestItemShipId = c.Guid(nullable: false),
                        RequestShipping_RequestShippingId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        StatusCode = c.Int(nullable: false),
                        ReqeustType = c.Int(nullable: false),
                        PriceValueType = c.Int(nullable: false),
                        PriceValue = c.Decimal(precision: 18, scale: 2),
                        PriceClientValueType = c.Int(nullable: false),
                        PriceClientValue = c.Decimal(precision: 18, scale: 2),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestItemShipId)
                .ForeignKey("dbo.RequestShippings", t => t.RequestShipping_RequestShippingId)
                .Index(t => t.RequestShipping_RequestShippingId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        AddressUser_CityCode = c.String(),
                        AddressUser_CityName = c.String(),
                        AddressUser_StreetCode = c.String(),
                        AddressUser_StreetName = c.String(),
                        AddressUser_ExtraDetail = c.String(),
                        AddressUser_StreetNum = c.String(),
                        AddressUser_IsSensor = c.Boolean(),
                        AddressUser_UID = c.Int(),
                        AddressUser_Lat = c.Double(),
                        AddressUser_Lng = c.Double(),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Tel = c.String(),
                        Organization_OrgId = c.Guid(),
                        IsActive = c.Boolean(),
                        Department = c.String(),
                        Subdivision = c.String(),
                        EmpId = c.String(),
                        DefaultView = c.Int(),
                        ViewAll = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Organization_OrgId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Guid(nullable: false),
                        Name = c.String(),
                        ProductNumber = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        IsCalculatingShippingInclusive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.ShippingItems",
                c => new
                    {
                        ShippingItemId = c.Guid(nullable: false),
                        Product_ProductId = c.Guid(),
                        Shipping_ShippingId = c.Guid(),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingItemId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .Index(t => t.Product_ProductId)
                .Index(t => t.Shipping_ShippingId);
            
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
                        OrderDirection = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StatusShippingId);
            
            CreateTable(
                "dbo.TimeLines",
                c => new
                    {
                        TimeLineId = c.Guid(nullable: false),
                        Shipping_ShippingId = c.Guid(),
                        Status = c.Int(nullable: false),
                        StatusShipping_StatusShippingId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        DescHtml = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        ApprovalRequest = c.Guid(),
                        ApprovalShip = c.Guid(),
                        OwnerShip = c.Guid(),
                    })
                .PrimaryKey(t => t.TimeLineId)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId)
                .ForeignKey("dbo.StatusShippings", t => t.StatusShipping_StatusShippingId)
                .Index(t => t.Shipping_ShippingId)
                .Index(t => t.StatusShipping_StatusShippingId);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountId = c.Guid(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Organizations_OrgId = c.Guid(),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        Name = c.String(),
                        Desc = c.String(),
                        IsSweeping = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DiscountId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Organizations_OrgId)
                .Index(t => t.ShippingCompany_ShippingCompanyId);
            
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        LeadId = c.Guid(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Tel = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LeadId);
            
            CreateTable(
                "dbo.ProductSystems",
                c => new
                    {
                        ProductSystemId = c.Guid(nullable: false),
                        Name = c.String(),
                        ProductKey = c.Int(nullable: false),
                        ProductTypeKey = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        SetDefaultValue = c.Boolean(),
                    })
                .PrimaryKey(t => t.ProductSystemId);
            
            CreateTable(
                "dbo.Slas",
                c => new
                    {
                        SlaId = c.Guid(nullable: false),
                        Priority = c.Int(nullable: false),
                        IsBusinessDay = c.Boolean(nullable: false),
                        Days = c.Double(nullable: false),
                        Hours = c.Double(nullable: false),
                        Mins = c.Double(nullable: false),
                        ShipType_ShipTypeId = c.Guid(),
                        ShippingCompany_ShippingCompanyId = c.Guid(),
                        Distance_DistanceId = c.Guid(),
                        Organizations_OrgId = c.Guid(),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SlaId)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId)
                .ForeignKey("dbo.Organizations", t => t.Organizations_OrgId)
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId)
                .ForeignKey("dbo.ShipTypes", t => t.ShipType_ShipTypeId)
                .Index(t => t.ShipType_ShipTypeId)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Distance_DistanceId)
                .Index(t => t.Organizations_OrgId);
            
            CreateTable(
                "dbo.TableTests",
                c => new
                    {
                        TableTestId = c.Guid(nullable: false),
                        ShippingAddress_CityCode = c.String(),
                        ShippingAddress_CityName = c.String(),
                        ShippingAddress_StreetCode = c.String(),
                        ShippingAddress_StreetName = c.String(),
                        ShippingAddress_ExtraDetail = c.String(),
                        ShippingAddress_StreetNum = c.String(),
                        ShippingAddress_IsSensor = c.Boolean(nullable: false),
                        ShippingAddress_UID = c.Int(nullable: false),
                        ShippingAddress_Lat = c.Double(nullable: false),
                        ShippingAddress_Lng = c.Double(nullable: false),
                        BillingAddress_CityCode = c.String(),
                        BillingAddress_CityName = c.String(),
                        BillingAddress_StreetCode = c.String(),
                        BillingAddress_StreetName = c.String(),
                        BillingAddress_ExtraDetail = c.String(),
                        BillingAddress_StreetNum = c.String(),
                        BillingAddress_IsSensor = c.Boolean(nullable: false),
                        BillingAddress_UID = c.Int(nullable: false),
                        BillingAddress_Lat = c.Double(nullable: false),
                        BillingAddress_Lng = c.Double(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TableTestId);
            
            CreateTable(
                "dbo.XbzCounters",
                c => new
                    {
                        XbzCounterId = c.Guid(nullable: false),
                        Name = c.String(),
                        LastNumber = c.Long(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.XbzCounterId);
            
            CreateTable(
                "dbo.OrganizationDistances",
                c => new
                    {
                        Organization_OrgId = c.Guid(nullable: false),
                        Distance_DistanceId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_OrgId, t.Distance_DistanceId })
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.Distance_DistanceId, cascadeDelete: true)
                .Index(t => t.Organization_OrgId)
                .Index(t => t.Distance_DistanceId);
            
            CreateTable(
                "dbo.ShippingCompanyOrganizations",
                c => new
                    {
                        ShippingCompany_ShippingCompanyId = c.Guid(nullable: false),
                        Organization_OrgId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShippingCompany_ShippingCompanyId, t.Organization_OrgId })
                .ForeignKey("dbo.ShippingCompanies", t => t.ShippingCompany_ShippingCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId, cascadeDelete: true)
                .Index(t => t.ShippingCompany_ShippingCompanyId)
                .Index(t => t.Organization_OrgId);
            
            CreateTable(
                "dbo.ApplicationUserShippings",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Shipping_ShippingId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Shipping_ShippingId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Shippings", t => t.Shipping_ShippingId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Shipping_ShippingId);
            
            CreateTable(
                "dbo.ProductOrganizations",
                c => new
                    {
                        Product_ProductId = c.Guid(nullable: false),
                        Organization_OrgId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ProductId, t.Organization_OrgId })
                .ForeignKey("dbo.Products", t => t.Product_ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrgId, cascadeDelete: true)
                .Index(t => t.Product_ProductId)
                .Index(t => t.Organization_OrgId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Slas", "ShipType_ShipTypeId", "dbo.ShipTypes");
            DropForeignKey("dbo.Slas", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.Slas", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Slas", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Discounts", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.Discounts", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.AttachmentShippings", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.TimeLines", "StatusShipping_StatusShippingId", "dbo.StatusShippings");
            DropForeignKey("dbo.TimeLines", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.Shippings", "StatusShipping_StatusShippingId", "dbo.StatusShippings");
            DropForeignKey("dbo.Shippings", "ShipType_ShipTypeId", "dbo.ShipTypes");
            DropForeignKey("dbo.Shippings", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.Shippings", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Shippings", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.ShippingItems", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.ShippingItems", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductOrganizations", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.ProductOrganizations", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.PriceLists", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.AspNetUsers", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.AspNetUsers", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.ApplicationUserShippings", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.ApplicationUserShippings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RequestShippings", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.RequestShippings", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.RequestItemShips", "RequestShipping_RequestShippingId", "dbo.RequestShippings");
            DropForeignKey("dbo.RequestShippings", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.ShippingCompanyOrganizations", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.ShippingCompanyOrganizations", "ShippingCompany_ShippingCompanyId", "dbo.ShippingCompanies");
            DropForeignKey("dbo.PriceLists", "Organizations_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.OrganizationDistances", "Distance_DistanceId", "dbo.Distances");
            DropForeignKey("dbo.OrganizationDistances", "Organization_OrgId", "dbo.Organizations");
            DropForeignKey("dbo.Comments", "Shipping_ShippingId", "dbo.Shippings");
            DropIndex("dbo.ProductOrganizations", new[] { "Organization_OrgId" });
            DropIndex("dbo.ProductOrganizations", new[] { "Product_ProductId" });
            DropIndex("dbo.ApplicationUserShippings", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.ApplicationUserShippings", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ShippingCompanyOrganizations", new[] { "Organization_OrgId" });
            DropIndex("dbo.ShippingCompanyOrganizations", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.OrganizationDistances", new[] { "Distance_DistanceId" });
            DropIndex("dbo.OrganizationDistances", new[] { "Organization_OrgId" });
            DropIndex("dbo.Slas", new[] { "Organizations_OrgId" });
            DropIndex("dbo.Slas", new[] { "Distance_DistanceId" });
            DropIndex("dbo.Slas", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.Slas", new[] { "ShipType_ShipTypeId" });
            DropIndex("dbo.Discounts", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.Discounts", new[] { "Organizations_OrgId" });
            DropIndex("dbo.TimeLines", new[] { "StatusShipping_StatusShippingId" });
            DropIndex("dbo.TimeLines", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.ShippingItems", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.ShippingItems", new[] { "Product_ProductId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Organization_OrgId" });
            DropIndex("dbo.AspNetUsers", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.RequestItemShips", new[] { "RequestShipping_RequestShippingId" });
            DropIndex("dbo.RequestShippings", new[] { "Organizations_OrgId" });
            DropIndex("dbo.RequestShippings", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.RequestShippings", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.PriceLists", new[] { "Organizations_OrgId" });
            DropIndex("dbo.PriceLists", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.Comments", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.Shippings", new[] { "Organization_OrgId" });
            DropIndex("dbo.Shippings", new[] { "StatusShipping_StatusShippingId" });
            DropIndex("dbo.Shippings", new[] { "Distance_DistanceId" });
            DropIndex("dbo.Shippings", new[] { "ShipType_ShipTypeId" });
            DropIndex("dbo.Shippings", new[] { "ShippingCompany_ShippingCompanyId" });
            DropIndex("dbo.AttachmentShippings", new[] { "Shipping_ShippingId" });
            DropTable("dbo.ProductOrganizations");
            DropTable("dbo.ApplicationUserShippings");
            DropTable("dbo.ShippingCompanyOrganizations");
            DropTable("dbo.OrganizationDistances");
            DropTable("dbo.XbzCounters");
            DropTable("dbo.TableTests");
            DropTable("dbo.Slas");
            DropTable("dbo.ProductSystems");
            DropTable("dbo.Leads");
            DropTable("dbo.Discounts");
            DropTable("dbo.TimeLines");
            DropTable("dbo.StatusShippings");
            DropTable("dbo.ShipTypes");
            DropTable("dbo.ShippingItems");
            DropTable("dbo.Products");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.RequestItemShips");
            DropTable("dbo.RequestShippings");
            DropTable("dbo.ShippingCompanies");
            DropTable("dbo.PriceLists");
            DropTable("dbo.Organizations");
            DropTable("dbo.Distances");
            DropTable("dbo.Comments");
            DropTable("dbo.Shippings");
            DropTable("dbo.AttachmentShippings");
        }
    }
}
