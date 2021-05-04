namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSetModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpectedRepsFst = c.Int(nullable: false),
                        ExpectedRepsSnd = c.Int(nullable: false),
                        ActualReps = c.Int(),
                        RestTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedWeight = c.String(nullable: false),
                        ActualWeight = c.Decimal(precision: 18, scale: 2),
                        Intensity = c.String(nullable: false),
                        Exercise_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exercises", t => t.Exercise_Id, cascadeDelete: true)
                .Index(t => t.Exercise_Id);
            
            DropColumn("dbo.Exercises", "ExpectedRepsFst");
            DropColumn("dbo.Exercises", "ExpectedRepsSnd");
            DropColumn("dbo.Exercises", "ActualReps");
            DropColumn("dbo.Exercises", "RestTime");
            DropColumn("dbo.Exercises", "ExpectedWeight");
            DropColumn("dbo.Exercises", "ActualWeight");
            DropColumn("dbo.Exercises", "Intensity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exercises", "Intensity", c => c.String(nullable: false));
            AddColumn("dbo.Exercises", "ActualWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exercises", "ExpectedWeight", c => c.String(nullable: false));
            AddColumn("dbo.Exercises", "RestTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Exercises", "ActualReps", c => c.Int());
            AddColumn("dbo.Exercises", "ExpectedRepsSnd", c => c.Int(nullable: false));
            AddColumn("dbo.Exercises", "ExpectedRepsFst", c => c.Int(nullable: false));
            DropForeignKey("dbo.Sets", "Exercise_Id", "dbo.Exercises");
            DropIndex("dbo.Sets", new[] { "Exercise_Id" });
            DropTable("dbo.Sets");
        }
    }
}
