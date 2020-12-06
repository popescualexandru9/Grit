namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullsFKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DailyWeight_Id", "dbo.Weights");
            DropIndex("dbo.AspNetUsers", new[] { "DailyWeight_Id" });
            AlterColumn("dbo.AspNetUsers", "DailyWeight_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "DailyWeight_Id");
            AddForeignKey("dbo.AspNetUsers", "DailyWeight_Id", "dbo.Weights", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DailyWeight_Id", "dbo.Weights");
            DropIndex("dbo.AspNetUsers", new[] { "DailyWeight_Id" });
            AlterColumn("dbo.AspNetUsers", "DailyWeight_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "DailyWeight_Id");
            AddForeignKey("dbo.AspNetUsers", "DailyWeight_Id", "dbo.Weights", "Id", cascadeDelete: true);
        }
    }
}
