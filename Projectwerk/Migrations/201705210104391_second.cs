namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicName = c.String(nullable: false),
                        PostAmount = c.Int(nullable: false),
                        ForumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fora", t => t.ForumId, cascadeDelete: true)
                .Index(t => t.ForumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topics", "ForumId", "dbo.Fora");
            DropIndex("dbo.Topics", new[] { "ForumId" });
            DropTable("dbo.Topics");
        }
    }
}
