namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pm4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PersonalMessages", "RecieverId", c => c.String());
            AlterColumn("dbo.PersonalMessages", "SenderId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonalMessages", "SenderId", c => c.Int(nullable: false));
            AlterColumn("dbo.PersonalMessages", "RecieverId", c => c.Int(nullable: false));
        }
    }
}
