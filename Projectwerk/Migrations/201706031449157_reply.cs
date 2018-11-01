namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ReplyToPostId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "ReplyToPostId");
        }
    }
}
