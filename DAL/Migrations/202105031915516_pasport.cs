namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pasport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Placements", "BirthDoc", c => c.String());
            AddColumn("dbo.Placements", "IsChild", c => c.Boolean(nullable: false));
            AddColumn("dbo.Placements", "IsChildBefore14", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Placements", "IsChildBefore14");
            DropColumn("dbo.Placements", "IsChild");
            DropColumn("dbo.Placements", "BirthDoc");
        }
    }
}
