namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wholocked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "WhoLocked", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "WhoLocked");
        }
    }
}
