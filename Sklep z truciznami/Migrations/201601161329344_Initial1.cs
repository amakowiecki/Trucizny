namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ratings", "ClientId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "ClientId", c => c.String(nullable: false));
        }
    }
}
