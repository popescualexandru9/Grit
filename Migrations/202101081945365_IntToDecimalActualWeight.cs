namespace Grit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntToDecimalActualWeight : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exercises", "ActualWeight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exercises", "ActualWeight", c => c.Int());
        }
    }
}
