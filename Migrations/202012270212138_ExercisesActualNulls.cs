namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExercisesActualNulls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exercises", "ActualReps", c => c.Int());
            AlterColumn("dbo.Exercises", "ActualWeight", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exercises", "ActualWeight", c => c.Int(nullable: false));
            AlterColumn("dbo.Exercises", "ActualReps", c => c.Int(nullable: false));
        }
    }
}
