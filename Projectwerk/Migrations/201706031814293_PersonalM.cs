namespace Projectwerk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonalM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonalMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecieverId = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        ThePM = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PersonalMessages");
        }
    }
}
