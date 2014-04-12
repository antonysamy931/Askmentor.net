namespace InternetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "LastName", c => c.String());
            AddColumn("dbo.UserProfile", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "PhoneNumber");
            DropColumn("dbo.UserProfile", "LastName");
        }
    }
}
