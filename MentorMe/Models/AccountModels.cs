using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace MentorMe.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        #region Added Code

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OAuthMembership> OAuthMembership { get; set; }
        public DbSet<UsersInRole> UserInRole { get; set; }
        public DbSet<ErrorLog> ErrorsLog { get; set; }
        public DbSet<PersonalDetail> PersonalDetails { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<AreaOfIntrest> AreaOfIntrests { get; set; }
        #endregion
    }

    #region Code-first model class second

    [Table("UserProfile", Schema = "dbo")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Provider { get; set; }
    }

    [Table("webpages_Membership", Schema = "dbo")]
    public class Membership
    {
        public Membership()
        {
            //Roles = new List<Role>();
            //OAuthMemberships = new List<OAuthMembership>();
            UsersInRoles = new List<UsersInRole>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(128)]
        public string ConfirmationToken { get; set; }
        public bool? IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        [Required, StringLength(128)]
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        [Required, StringLength(128)]
        public string PasswordSalt { get; set; }
        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        //public ICollection<Role> Roles { get; set; }

        //[ForeignKey("UserId")]
        //public ICollection<OAuthMembership> OAuthMemberships { get; set; }

        [ForeignKey("UserId")]
        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

    [Table("webpages_OAuthMembership", Schema = "dbo")]
    public class OAuthMembership
    {
        [Key, Column(Order = 0), StringLength(30)]
        public string Provider { get; set; }

        [Key, Column(Order = 1), StringLength(100)]
        public string ProviderUserId { get; set; }

        public int UserId { get; set; }

        //[Column("UserId"), InverseProperty("OAuthMemberships")]
        //public Membership User { get; set; }
    }

    [Table("webpages_UsersInRoles", Schema = "dbo")]
    public class UsersInRole
    {
        [Key, Column(Order = 0)]
        public int RoleId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [Column("RoleId"), InverseProperty("UsersInRoles")]
        public Role Roles { get; set; }

        [Column("UserId"), InverseProperty("UsersInRoles")]
        public Membership Members { get; set; }
    }

    [Table("webpages_Roles", Schema = "dbo")]
    public class Role
    {
        public Role()
        {
            UsersInRoles = new List<UsersInRole>();
        }

        [Key]
        public int RoleId { get; set; }
        [StringLength(256)]
        public string RoleName { get; set; }
        [ForeignKey("RoleId")]
        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

    [Table("Error_log", Schema = "dbo")]
    public class ErrorLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ErrorId { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ErrorLogDate { get; set; }
    }

    [Table("User_areaofintrest", Schema = "dbo")]
    public class AreaOfIntrest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InterestId { get; set; }
        public int UserId { get; set; }
        public string Experience { get; set; }
        public string KeySkills { get; set; }
    }

    [Table("User_education", Schema = "dbo")]
    public class Education
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EducationId { get; set; }
        public int UserId { get; set; }
        public string Graduation { get; set; }
        public string EducationMode { get; set; }
        public string Specialization { get; set; }
        public string University { get; set; }
        public string YearComplete { get; set; }
        public string PostGraduation { get; set; }
        public string PostEducationMode { get; set; }
        public string PostSpecialization { get; set; }
        public string PostUnversity { get; set; }
        public string PostYearComplete { get; set; }
        public string Doctorate { get; set; }
        public string DoctorateEducationMode { get; set; }
        public string DoctorateSpecialization { get; set; }
        public string DoctorateUniversity { get; set; }
        public string DoctorateYearComplete { get; set; }
        public string Certification { get; set; }
        public string Grade { get; set; }
        public string CourseComplete { get; set; }
    }

    [Table("User_Detail", Schema = "dbo")]
    public class PersonalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonalId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string MobileNumber { get; set; }
        public string ExtentionCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternateEmail { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
    #endregion

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "Enter valid email address")]
        [Remote("IsUserNameAvailable", "Validation")]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class ModelEducation
    {
        private string _Graduation;
        private string _EducationMode;
        private List<string> _Modes;
        private string _Specialization;
        private string _University;
        private string _YearComplete;
        private string _PostGraduation;
        private string _PostGraduationEducationMode;
        private string _PostSpecialization;
        private string _PostUniversity;
        private string _PostYearComplete;
        private string _Doctorate;
        private string _DoctorateEducationMode;
        private string _DoctorateSpecialization;
        private string _DoctorateUniversity;
        private string _DoctorateYearComplete;
        private string _Certification;
        private string _Grade;

        [Display(Name = "Under Graduation")]
        public string Graduation
        {
            get
            {
                return this._Graduation;
            }
            set
            {
                this._Graduation = value;
            }
        }

        public string EducationMode
        {
            get { return this._EducationMode; }
            set { this._EducationMode = value; }
        }

        public List<string> Modes
        {
            get { return this._Modes; }
            set { this._Modes = new List<string>() { "Full Time", "Part Time", "Correspond" }; }
        }

        [Display(Name = "Specialization")]
        public string Specialization
        {
            get { return this._Specialization; }
            set { this._Specialization = value; }
        }

        [Display(Name = "University")]
        public string University
        {
            get { return this._University; }
            set { this._University = value; }
        }

        [Display(Name = "Year Of Complete")]
        public string YearComplete
        {
            get { return this._YearComplete; }
            set { this._YearComplete = value; }
        }

        [Display(Name = "Post Graducation")]
        public string PostGraduation
        {
            get { return this._PostGraduation; ; }
            set { this._PostGraduation = value; }
        }

        [Display(Name = "Education Mode")]
        public string PostGraduationEducationMode
        {
            get { return this._PostGraduationEducationMode; }
            set { this._PostGraduationEducationMode = value; }
        }

        [Display(Name = "Specialization")]
        public string PostSpecialization
        {
            get { return this._PostSpecialization; }
            set { this._PostSpecialization = value; }
        }

        [Display(Name = "University")]
        public string PostUniversity
        {
            get { return this._PostUniversity; }
            set { this._PostUniversity = value; }
        }

        [Display(Name = "Year Of Complete")]
        public string PostYearComplete
        {
            get { return this._PostYearComplete; }
            set { this._PostYearComplete = value; }
        }

        [Display(Name = "Doctorate / Ph.d")]
        public string Doctorate
        {
            get { return this._Doctorate; }
            set { this._Doctorate = value; }
        }

        [Display(Name = "Education Mode")]
        public string DoctorateEducationMode
        {
            get { return this._DoctorateEducationMode; }
            set { this._DoctorateEducationMode = value; }
        }

        [Display(Name = "Specialization")]
        public string DoctorateSpecialization
        {
            get { return this._DoctorateSpecialization; }
            set { this._DoctorateSpecialization = value; }
        }

        [Display(Name = "University")]
        public string DoctorateUniversity
        {
            get { return this._DoctorateUniversity; }
            set { this._DoctorateUniversity = value; }
        }

        [Display(Name = "Year Of Complete")]
        public string DoctorateYearComplete
        {
            get { return this._DoctorateYearComplete; }
            set { this._DoctorateYearComplete = value; }
        }

        [Display(Name = "Certification")]
        public string Certification
        {
            get { return this._Certification; }
            set { this._Certification = value; }
        }

        [Display(Name = "Grade")]
        public string Grade
        {
            get { return this._Grade; }
            set { this._Grade = value; }
        }
    }

    public class ModelAreaOfInterest
    {
        private string _KeySkills;
        private string _Experiance;

        [Display(Name = "Area Of Interest")]
        public string KeySkill
        {
            get { return this._KeySkills; }
            set { this._KeySkills = value; }
        }

        [Display(Name = "Experiance")]
        public string Experiance
        {
            get { return this._Experiance; }
            set { this._Experiance = value; }
        }
    }

    public class ModelPerson
    {
        private string _PersonName;
        private string _DateOfBirth;
        private bool _Gender;
        private string _Marital;
        private List<string> _MaritalCollection;
        private string _MobileNumber;
        private string _ExtentionCode;
        private string _PhoneNumber;
        private string _AlternateEmail;
        private string _AddressOne;
        private string _AddressTwo;
        private string _City;
        private string _State;
        private string _Country;
        private string _PostalCode;

        [Display(Name = "Name")]
        public string PersonName
        {
            get { return this._PersonName; }
            set { this._PersonName = value; }
        }

        [Display(Name = "Date Of Birth")]
        public string DateOfBirth
        {
            get { return this._DateOfBirth; }
            set { this._DateOfBirth = value; }
        }

        [Display(Name = "Gender")]
        public bool Gender
        {
            get { return this._Gender; }
            set { this._Gender = value; }
        }

        [Display(Name = "Marital Status")]
        public string Marital
        {
            get { return this._Marital; }
            set { this._Marital = value; }
        }

        public List<string> MaritalCollection
        {
            get
            {
                return this._MaritalCollection;
            }
            set
            {
                this._MaritalCollection = new List<string>() { "Single", "Married" };
            }
        }

        [Display(Name = "Mobile")]
        public string MobileNumber
        {
            get { return this._MobileNumber; }
            set { this._MobileNumber = value; }
        }

        [Display(Name = "Phone Number")]
        public string ExtentionCode
        {
            get { return this._ExtentionCode; }
            set { this._ExtentionCode = value; }
        }
        public string PhoneNumber
        {
            get { return this._PhoneNumber; }
            set { this._PhoneNumber = value; }
        }

        [Display(Name = "Alternate Email")]
        [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "Please enter valid email address.")]
        public string AlternateEmail
        {
            get { return this._AlternateEmail; }
            set { this._AlternateEmail = value; }
        }

        [Display(Name = "Address Line One")]
        public string AddressOne
        {
            get { return this._AddressOne; }
            set { this._AddressOne = value; }
        }

        [Display(Name = "Address Line Two")]
        public string AddressTwo
        {
            get { return this._AddressTwo; }
            set { this._AddressTwo = value; }
        }

        [Display(Name = "City")]
        public string City
        {
            get { return this._City; }
            set { this._City = value; }
        }

        [Display(Name = "State")]
        public string State
        {
            get { return this._State; }
            set { this._State = value; }
        }

        [Display(Name = "Country")]
        public string Country
        {
            get { return this._Country; }
            set { this._Country = value; }
        }

        [Display(Name = "Postal Code")]
        public string PostalCode
        {
            get { return this._PostalCode; }
            set { this._PostalCode = value; }
        }
    }
}
