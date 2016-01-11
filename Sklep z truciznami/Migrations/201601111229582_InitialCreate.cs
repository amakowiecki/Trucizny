namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        ClientId = c.String(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Quantity = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        AnswerDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
