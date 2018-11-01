namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postchange2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "ForumUserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "ForumUserName", c => c.String());
        }
    }
}
