namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserIdToWeightTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Weights", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Weights", "UserId");
        }
    }
}
