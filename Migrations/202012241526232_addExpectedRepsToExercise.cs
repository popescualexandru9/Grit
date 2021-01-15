namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addExpectedRepsToExercise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "ExpectedRepsFst", c => c.Int(nullable: false));
            AddColumn("dbo.Exercises", "ExpectedRepsSnd", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "ExpectedRepsSnd");
            DropColumn("dbo.Exercises", "ExpectedRepsFst");
        }
    }
}
