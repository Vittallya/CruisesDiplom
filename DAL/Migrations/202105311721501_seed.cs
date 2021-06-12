namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seed : DbMigration
    {
        public override void Up()
        {
            Sql(Properties.Resources.seed);
        }
        
        public override void Down()
        {
        }
    }
}
