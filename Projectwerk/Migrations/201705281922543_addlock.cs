namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "Locked");
        }
    }
}
