namespace MentorMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Provider = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 30),
                        ProviderUserId = c.String(nullable: false, maxLength: 100),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId });
            
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
            
            CreateTable(
                "dbo.User_Detail",
                c => new
                    {
                        PersonalId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        DateOfBirth = c.String(),
                        Gender = c.String(),
                        MaritalStatus = c.String(),
                        MobileNumber = c.String(),
                        ExtentionCode = c.String(),
                        PhoneNumber = c.String(),
                        AlternateEmail = c.String(),
                        AddressLineOne = c.String(),
                        AddressLineTwo = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.PersonalId);
            
            CreateTable(
                "dbo.User_education",
                c => new
                    {
                        EducationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Graduation = c.String(),
                        EducationMode = c.String(),
                        Specialization = c.String(),
                        University = c.String(),
                        YearComplete = c.String(),
                        PostGraduation = c.String(),
                        PostEducationMode = c.String(),
                        PostSpecialization = c.String(),
                        PostUnversity = c.String(),
                        PostYearComplete = c.String(),
                        Doctorate = c.String(),
                        DoctorateEducationMode = c.String(),
                        DoctorateSpecialization = c.String(),
                        DoctorateUniversity = c.String(),
                        DoctorateYearComplete = c.String(),
                        Certification = c.String(),
                        Grade = c.String(),
                        CourseComplete = c.String(),
                    })
                .PrimaryKey(t => t.EducationId);
            
            CreateTable(
                "dbo.User_areaofintrest",
                c => new
                    {
                        InterestId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Experience = c.String(),
                        KeySkills = c.String(),
                    })
                .PrimaryKey(t => t.InterestId);
            
            CreateTable(
                "dbo.User_feedback",
                c => new
                    {
                        FeedId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Rating = c.String(),
                        Comments = c.String(),
                        CurrentDate = c.String(),
                    })
                .PrimaryKey(t => t.FeedId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropTable("dbo.User_feedback");
            DropTable("dbo.User_areaofintrest");
            DropTable("dbo.User_education");
            DropTable("dbo.User_Detail");
            DropTable("dbo.Error_log");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.UserProfile");
        }
    }
}
