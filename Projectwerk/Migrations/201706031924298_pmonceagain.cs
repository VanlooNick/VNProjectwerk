namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pmonceagain : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PersonalMessages", "RecieverName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonalMessages", "RecieverName", c => c.String());
        }
    }
}
