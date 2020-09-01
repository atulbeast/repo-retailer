namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wishlist_change_userId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WishLists", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WishLists", "UserId", c => c.Int(nullable: false));
        }
    }
}
