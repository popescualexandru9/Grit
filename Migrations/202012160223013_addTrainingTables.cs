namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTrainingTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ActualReps = c.Int(nullable: false),
                        RestTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualWeight = c.Int(nullable: false),
                        Intensity = c.String(nullable: false),
                        MuscleGroup = c.String(nullable: false),
                        Workout_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workouts", t => t.Workout_Id)
                .Index(t => t.Workout_Id);
            
            CreateTable(
                "dbo.TrainingSplits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Frequency = c.Int(nullable: false),
                        Equipment = c.String(nullable: false),
                        Goal = c.String(nullable: false),
                        Experience = c.String(nullable: false),
                        Length = c.Int(nullable: false),
                        Workout_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workouts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Exercise_Id = c.Int(),
                        TrainingSplit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TrainingSplits", t => t.TrainingSplit_Id)
                .Index(t => t.TrainingSplit_Id);
            
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
            
            AddColumn("dbo.AspNetUsers", "TrainingSplit_Id", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workouts", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropForeignKey("dbo.Exercises", "Workout_Id", "dbo.Workouts");
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropForeignKey("dbo.ApplicationUserTrainingSplits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "TrainingSplit_Id" });
            DropIndex("dbo.ApplicationUserTrainingSplits", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Workouts", new[] { "TrainingSplit_Id" });
            DropIndex("dbo.Exercises", new[] { "Workout_Id" });
            DropColumn("dbo.AspNetUsers", "TrainingSplit_Id");
            DropTable("dbo.ApplicationUserTrainingSplits");
            DropTable("dbo.Workouts");
            DropTable("dbo.TrainingSplits");
            DropTable("dbo.Exercises");
        }
    }
}
