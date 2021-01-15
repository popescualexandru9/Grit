namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapM2M : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "TrainingSplit_Id" });
            CreateTable(
                "dbo.UserSplits",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        SplitID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.SplitID })
                .ForeignKey("dbo.TrainingSplits", t => t.SplitID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.SplitID);
            
            DropTable("dbo.ApplicationUserTrainingSplits");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserTrainingSplits",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        TrainingSplit_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.TrainingSplit_Id });
            
            DropForeignKey("dbo.UserSplits", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSplits", "SplitID", "dbo.TrainingSplits");
            DropIndex("dbo.UserSplits", new[] { "SplitID" });
            DropIndex("dbo.UserSplits", new[] { "UserID" });
            DropTable("dbo.UserSplits");
            CreateIndex("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id");
            CreateIndex("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id", "dbo.TrainingSplits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
