namespace MentorMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class errorlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Error_log",
                c => new
                    {
                        ErrorId = c.Int(nullable: false, identity: true),
                        ExceptionMessage = c.String(),
                        ExceptionStackTrace = c.String(),
                        ErrorLogDate = c.String(),
                    })
                .PrimaryKey(t => t.ErrorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Error_log");
        }
    }
}
