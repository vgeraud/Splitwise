namespace Splitwise.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        InitialAmount = c.Double(nullable: false),
                        CurrentAmount = c.Double(nullable: false),
                        Currency = c.Int(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        Payer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Payer_Id)
                .Index(t => t.Payer_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Currency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category = c.Int(nullable: false),
                        CurrentBalance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "Payer_Id", "dbo.Users");
            DropIndex("dbo.Expenses", new[] { "Payer_Id" });
            DropTable("dbo.Groups");
            DropTable("dbo.Users");
            DropTable("dbo.Expenses");
        }
    }
}
