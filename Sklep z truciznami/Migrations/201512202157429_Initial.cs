namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        SupplierId = c.String(),
                        ProductName = c.String(nullable: false),
                        ProductDescription = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        Category = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Tags = c.String(nullable: false),
                        RatingSum = c.Int(nullable: false),
                        RatingNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
