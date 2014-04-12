namespace InternetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Provider", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Provider");
        }
    }
}
