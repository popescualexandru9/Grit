namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveWorkoutId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ActiveWorkout_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ActiveWorkout_Id");
            AddForeignKey("dbo.AspNetUsers", "ActiveWorkout_Id", "dbo.TrainingSplits", "Id");
            DropColumn("dbo.AspNetUsers", "ActiveWorkout");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ActiveWorkout", c => c.Int());
            DropForeignKey("dbo.AspNetUsers", "ActiveWorkout_Id", "dbo.TrainingSplits");
            DropIndex("dbo.AspNetUsers", new[] { "ActiveWorkout_Id" });
            DropColumn("dbo.AspNetUsers", "ActiveWorkout_Id");
        }
    }
}
