namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjustpost : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "TopicName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "TopicName", c => c.String());
        }
    }
}
