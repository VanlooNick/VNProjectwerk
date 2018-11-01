namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetopic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "CreatedBy", c => c.String());
            DropColumn("dbo.Topics", "PostAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Topics", "PostAmount", c => c.Int(nullable: false));
            DropColumn("dbo.Topics", "CreatedBy");
        }
    }
}
