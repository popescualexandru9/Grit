namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addConstructors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "TrainingSplit_Id" });
            AddColumn("dbo.TrainingSplits", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ActiveWorkout", c => c.Int());
            CreateIndex("dbo.TrainingSplits", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "ActiveWorkout");
            CreateIndex("dbo.AspNetUsers", "TrainingSplit_Id");
            AddForeignKey("dbo.AspNetUsers", "ActiveWorkout", "dbo.TrainingSplits", "Id");
            AddForeignKey("dbo.TrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "TrainingSplit_Id", "dbo.TrainingSplits", "Id");
            DropColumn("dbo.TrainingSplits", "Active");
            DropColumn("dbo.TrainingSplits", "Workout_Id");
            DropColumn("dbo.Workouts", "Exercise_Id");
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
            
            AddColumn("dbo.Workouts", "Exercise_Id", c => c.Int());
            AddColumn("dbo.TrainingSplits", "Workout_Id", c => c.Int());
            AddColumn("dbo.TrainingSplits", "Active", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AspNetUsers", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropForeignKey("dbo.TrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ActiveWorkout", "dbo.TrainingSplits");
            DropIndex("dbo.AspNetUsers", new[] { "TrainingSplit_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ActiveWorkout" });
            DropIndex("dbo.TrainingSplits", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "ActiveWorkout");
            DropColumn("dbo.TrainingSplits", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id");
            CreateIndex("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id", "dbo.TrainingSplits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
