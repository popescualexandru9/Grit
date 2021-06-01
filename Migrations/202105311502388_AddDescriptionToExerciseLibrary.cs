namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToExerciseLibrary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExerciseLibraries", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExerciseLibraries", "Description");
        }
    }
}
