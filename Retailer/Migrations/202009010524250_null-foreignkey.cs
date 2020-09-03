namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullforeignkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Orders", "PaymentId", "dbo.Payments");
            DropIndex("dbo.Orders", new[] { "AddressId" });
            DropIndex("dbo.Orders", new[] { "PaymentId" });
            AlterColumn("dbo.Orders", "AddressId", c => c.Long());
            AlterColumn("dbo.Orders", "PaymentId", c => c.Long());
            CreateIndex("dbo.Orders", "AddressId");
            CreateIndex("dbo.Orders", "PaymentId");
            AddForeignKey("dbo.Orders", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Orders", "PaymentId", "dbo.Payments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Orders", new[] { "PaymentId" });
            DropIndex("dbo.Orders", new[] { "AddressId" });
            AlterColumn("dbo.Orders", "PaymentId", c => c.Long(nullable: false));
            AlterColumn("dbo.Orders", "AddressId", c => c.Long(nullable: false));
            CreateIndex("dbo.Orders", "PaymentId");
            CreateIndex("dbo.Orders", "AddressId");
            AddForeignKey("dbo.Orders", "PaymentId", "dbo.Payments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "AddressId", "dbo.Addresses", "Id", cascadeDelete: true);
        }
    }
}
