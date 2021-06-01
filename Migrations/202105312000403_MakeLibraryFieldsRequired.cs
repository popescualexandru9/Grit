namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeLibraryFieldsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExerciseLibraries", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ExerciseLibraries", "Url", c => c.String(nullable: false));
            AlterColumn("dbo.ExerciseLibraries", "MuscleGroup", c => c.String(nullable: false));
            AlterColumn("dbo.ExerciseLibraries", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExerciseLibraries", "Description", c => c.String());
            AlterColumn("dbo.ExerciseLibraries", "MuscleGroup", c => c.String());
            AlterColumn("dbo.ExerciseLibraries", "Url", c => c.String());
            AlterColumn("dbo.ExerciseLibraries", "Name", c => c.String());
        }
    }
}
