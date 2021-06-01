namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addExercisesLibrary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExerciseLibraries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        MuscleGroup = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExerciseLibraries");
        }
    }
}
