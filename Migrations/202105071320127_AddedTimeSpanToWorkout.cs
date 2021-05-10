namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimeSpanToWorkout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workouts", "TimeSpan", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workouts", "TimeSpan");
        }
    }
}
