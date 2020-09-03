namespace Retailer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profile_foreignkey_user : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Profiles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Profiles", new[] { "UserId" });
            AddColumn("dbo.AspNetUsers", "ProfileId", c => c.Long());
            AddColumn("dbo.Profiles", "Gender", c => c.String());
            AddColumn("dbo.Profiles", "Email", c => c.String());
            AddColumn("dbo.Profiles", "DOB", c => c.String());
            CreateIndex("dbo.AspNetUsers", "ProfileId");
            AddForeignKey("dbo.AspNetUsers", "ProfileId", "dbo.Profiles", "Id");
            DropColumn("dbo.Profiles", "EmailID");
            DropColumn("dbo.Profiles", "Age");
            DropColumn("dbo.Profiles", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Profiles", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "EmailID", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.AspNetUsers", new[] { "ProfileId" });
            DropColumn("dbo.Profiles", "DOB");
            DropColumn("dbo.Profiles", "Email");
            DropColumn("dbo.Profiles", "Gender");
            DropColumn("dbo.AspNetUsers", "ProfileId");
            CreateIndex("dbo.Profiles", "UserId");
            AddForeignKey("dbo.Profiles", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
