namespace Michal.Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddADDR : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoreAddress",
                c => new
                    {
                        MoreAddressId = c.Guid(nullable: false),
                        ObjectId = c.Guid(nullable: false),
                        ObjectTableCode = c.Int(nullable: false),
                        Address_CityCode = c.String(),
                        Address_CityName = c.String(),
                        Address_StreetCode = c.String(),
                        Address_StreetName = c.String(),
                        Address_ExtraDetail = c.String(),
                        Address_StreetNum = c.String(),
                        Address_IsSensor = c.Boolean(nullable: false),
                        Address_UID = c.Int(nullable: false),
                        Address_Lat = c.Double(nullable: false),
                        Address_Lng = c.Double(nullable: false),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        CreatedBy = c.Guid(),
                        ModifiedBy = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MoreAddressId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MoreAddress");
        }
    }
}
