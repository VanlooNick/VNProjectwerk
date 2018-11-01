namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourth : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Fora", "TopicAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fora", "TopicAmount", c => c.Int(nullable: false));
        }
    }
}
