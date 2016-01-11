namespace Sklep_z_truciznami.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "AnswerDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "AnswerDate", c => c.DateTime(nullable: false));
        }
    }
}
