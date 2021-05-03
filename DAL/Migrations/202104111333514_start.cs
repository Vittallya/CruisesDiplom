namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class start : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        FullCost = c.Double(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        PeopleCount = c.Int(nullable: false),
                        ChildCount = c.Int(nullable: false),
                        PunktNaznacheniya = c.String(),
                        OrderStatus = c.Int(nullable: false),
                        Tour_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.Tour_Id)
                .Index(t => t.ClientId)
                .Index(t => t.Tour_Id);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Desctiprion = c.String(),
                        Cost = c.Double(nullable: false),
                        ChildCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Orders", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Profiles", "Id", "dbo.Clients");
            DropIndex("dbo.Orders", new[] { "Tour_Id" });
            DropIndex("dbo.Orders", new[] { "ClientId" });
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropTable("dbo.Tours");
            DropTable("dbo.Orders");
            DropTable("dbo.Profiles");
            DropTable("dbo.Clients");
        }
    }
}
