namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repairRelationSchemaForReal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Workouts", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropForeignKey("dbo.Exercises", "Workout_Id", "dbo.Workouts");
            DropIndex("dbo.Exercises", new[] { "Workout_Id" });
            DropIndex("dbo.Workouts", new[] { "TrainingSplit_Id" });
            AlterColumn("dbo.Exercises", "Workout_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Workouts", "TrainingSplit_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Exercises", "Workout_Id");
            CreateIndex("dbo.Workouts", "TrainingSplit_Id");
            AddForeignKey("dbo.Workouts", "TrainingSplit_Id", "dbo.TrainingSplits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Exercises", "Workout_Id", "dbo.Workouts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exercises", "Workout_Id", "dbo.Workouts");
            DropForeignKey("dbo.Workouts", "TrainingSplit_Id", "dbo.TrainingSplits");
            DropIndex("dbo.Workouts", new[] { "TrainingSplit_Id" });
            DropIndex("dbo.Exercises", new[] { "Workout_Id" });
            AlterColumn("dbo.Workouts", "TrainingSplit_Id", c => c.Int());
            AlterColumn("dbo.Exercises", "Workout_Id", c => c.Int());
            CreateIndex("dbo.Workouts", "TrainingSplit_Id");
            CreateIndex("dbo.Exercises", "Workout_Id");
            AddForeignKey("dbo.Exercises", "Workout_Id", "dbo.Workouts", "Id");
            AddForeignKey("dbo.Workouts", "TrainingSplit_Id", "dbo.TrainingSplits", "Id");
        }
    }
}
