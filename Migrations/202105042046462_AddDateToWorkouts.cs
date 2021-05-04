namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateToWorkouts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workouts", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workouts", "Date");
        }
    }
}
