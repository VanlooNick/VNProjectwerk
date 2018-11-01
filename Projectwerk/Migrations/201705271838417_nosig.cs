namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nosig : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Signature");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Signature", c => c.String());
        }
    }
}
