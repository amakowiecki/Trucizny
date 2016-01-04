namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductDescription", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductDescription", c => c.String(nullable: false));
        }
    }
}
