namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wishlist_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishLists", "ProductId", c => c.Long(nullable: false));
            CreateIndex("dbo.WishLists", "ProductId");
            AddForeignKey("dbo.WishLists", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            DropColumn("dbo.WishLists", "SubCategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WishLists", "SubCategoryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.WishLists", "ProductId", "dbo.Products");
            DropIndex("dbo.WishLists", new[] { "ProductId" });
            DropColumn("dbo.WishLists", "ProductId");
        }
    }
}
