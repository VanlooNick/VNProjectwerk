namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pm2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalMessages", "RecieverName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalMessages", "RecieverName");
        }
    }
}
