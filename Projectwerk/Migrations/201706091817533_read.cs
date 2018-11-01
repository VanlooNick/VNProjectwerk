namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class read : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalMessages", "Read", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalMessages", "Read");
        }
    }
}
