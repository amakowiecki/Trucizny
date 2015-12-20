namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PhotoImageFileName", c => c.String());
            AddColumn("dbo.Products", "PhotoImageMimeType", c => c.String());
            AddColumn("dbo.Products", "PhotoFile", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "PhotoFile");
            DropColumn("dbo.Products", "PhotoImageMimeType");
            DropColumn("dbo.Products", "PhotoImageFileName");
        }
    }
}
