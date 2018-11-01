namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postchange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PostedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "PostedBy");
        }
    }
}
