namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diplom2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tours", "StartPlace", c => c.String());
            DropColumn("dbo.Orders", "StartDate");
            DropColumn("dbo.Orders", "EndDate");
            DropColumn("dbo.Orders", "PunktNaznacheniya");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "PunktNaznacheniya", c => c.String());
            AddColumn("dbo.Orders", "EndDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Orders", "StartDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Tours", "StartPlace");
            DropColumn("dbo.Tours", "StartDate");
        }
    }
}
