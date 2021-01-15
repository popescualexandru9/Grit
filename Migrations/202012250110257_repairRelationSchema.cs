namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repairRelationSchema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ActiveWorkout", "dbo.TrainingSplits");
            DropForeignKey("dbo.TrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropIndex("dbo.TrainingSplits", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ActiveWorkout" });
            DropIndex("dbo.AspNetUsers", new[] { "TrainingSplit_Id" });
            CreateTable(
                "dbo.ApplicationUserTrainingSplits",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        TrainingSplit_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.TrainingSplit_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.TrainingSplits", t => t.TrainingSplit_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.TrainingSplit_Id);
            
            DropColumn("dbo.TrainingSplits", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "TrainingSplit_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TrainingSplit_Id", c => c.Int());
            AddColumn("dbo.TrainingSplits", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "TrainingSplit_Id" });
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserTrainingSplits");
            CreateIndex("dbo.AspNetUsers", "TrainingSplit_Id");
            CreateIndex("dbo.AspNetUsers", "ActiveWorkout");
            CreateIndex("dbo.TrainingSplits", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "TrainingSplit_Id", "dbo.TrainingSplits", "Id");
            AddForeignKey("dbo.TrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "ActiveWorkout", "dbo.TrainingSplits", "Id");
        }
    }
}
