namespace MentorMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class threetable : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User_areaofintrest");
            DropTable("dbo.User_education");
            DropTable("dbo.User_Detail");
        }
    }
}
