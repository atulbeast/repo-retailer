namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datecol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "UpdatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "UpdatedAt");
            DropColumn("dbo.Orders", "CreatedAt");
        }
    }
}
