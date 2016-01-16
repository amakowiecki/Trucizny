namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RateId = c.Int(nullable: false, identity: true),
                        ClientId = c.String(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RateId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ratings");
        }
    }
}
