namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subj : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalMessages", "Subject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonalMessages", "Subject");
        }
    }
}
