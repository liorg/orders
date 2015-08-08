namespace sln.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changescema : DbMigration
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
            
            AddColumn("dbo.Cities", "CityCode", c => c.String());
            AddColumn("dbo.Shippings", "CityFromName", c => c.String());
            AddColumn("dbo.Shippings", "CityToName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserShippings", "Shipping_ShippingId", "dbo.Shippings");
            DropForeignKey("dbo.ApplicationUserShippings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AttachmentShippings", "Shipping_ShippingId", "dbo.Shippings");
            DropIndex("dbo.ApplicationUserShippings", new[] { "Shipping_ShippingId" });
            DropIndex("dbo.ApplicationUserShippings", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AttachmentShippings", new[] { "Shipping_ShippingId" });
            DropColumn("dbo.Shippings", "CityToName");
            DropColumn("dbo.Shippings", "CityFromName");
            DropColumn("dbo.Cities", "CityCode");
            DropTable("dbo.ApplicationUserShippings");
            DropTable("dbo.AttachmentShippings");
        }
    }
}
