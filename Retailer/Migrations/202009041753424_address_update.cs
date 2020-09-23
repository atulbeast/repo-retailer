namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class address_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Locality", c => c.String());
            AddColumn("dbo.Addresses", "HouseNo", c => c.String());
            AddColumn("dbo.Addresses", "Floor", c => c.String());
            AddColumn("dbo.Addresses", "Landmark", c => c.String());
            AddColumn("dbo.Addresses", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Landmark");
            DropColumn("dbo.Addresses", "Floor");
            DropColumn("dbo.Addresses", "HouseNo");
            DropColumn("dbo.Addresses", "Locality");
            DropColumn("dbo.Addresses", "Type");
        }
    }
}
