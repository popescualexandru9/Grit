namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trainingActiveField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrainingSplits", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrainingSplits", "Active");
        }
    }
}
