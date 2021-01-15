namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expectedWeightToString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "ExpectedWeight", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "ExpectedWeight");
        }
    }
}
