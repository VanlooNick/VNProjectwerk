namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletepost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "isDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "isDeleted");
        }
    }
}
