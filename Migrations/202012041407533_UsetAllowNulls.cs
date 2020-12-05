namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsetAllowNulls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Height", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.AspNetUsers", "Birthdate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "SignUpDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "SignUpDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Birthdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Height", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
