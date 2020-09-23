namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class item_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Type");
        }
    }
}
