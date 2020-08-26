namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppImage_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppImages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Url = c.String(),
                        ItemType = c.Int(nullable: false),
                        ItemId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemTypes", t => t.ItemType, cascadeDelete: true)
                .Index(t => t.ItemType);
            
            CreateTable(
                "dbo.ItemTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppImages", "ItemType", "dbo.ItemTypes");
            DropIndex("dbo.AppImages", new[] { "ItemType" });
            DropTable("dbo.ItemTypes");
            DropTable("dbo.AppImages");
        }
    }
}
