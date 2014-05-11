using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MentorMe.Models;

namespace MentorMe.Controllers
{
    public class DetailController : BaseController
    {
        [HttpGet]
        public ActionResult Personal()
        {
            ModelPerson oPerson = new ModelPerson();
            var oUser = this.model.UserProfiles.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (oUser != null)
            {
                oPerson.PersonName = oUser.FirstName + " " + oUser.LastName;
                oPerson.MobileNumber = oUser.PhoneNumber;
                var personal = this.model.PersonalDetails.Where(x => x.UserId == oUser.UserId).FirstOrDefault();
                if (personal != null)
                {
                    oPerson.AddressOne = personal.AddressLineOne;
                    oPerson.AddressTwo = personal.AddressLineTwo;
                    oPerson.AlternateEmail = personal.AlternateEmail;
                    oPerson.City = personal.City;
                    oPerson.Country = personal.Country;
                    oPerson.DateOfBirth = personal.DateOfBirth;
                    oPerson.ExtentionCode = personal.ExtentionCode;
                    if (personal.Gender == "Male")
                    {
                        oPerson.Gender = true;
                    }
                    else
                    {
                        oPerson.Gender = false;
                    }
                    oPerson.Marital = personal.MaritalStatus;
                    oPerson.MobileNumber = personal.MobileNumber;
                    if (personal.Name != null)
                    {
                        oPerson.PersonName = personal.Name;
                    }
                    if (personal.PhoneNumber != null)
                    {
                        oPerson.PhoneNumber = personal.PhoneNumber;
                    }
                    oPerson.PostalCode = personal.PostalCode;
                    oPerson.State = personal.State;
                }
            }
            return View(oPerson);
        }

        [HttpPost]
        public ActionResult Personal(ModelPerson oModel)
        {
            if (ModelState.IsValid)
            {
                var oUser = this.model.UserProfiles.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                if (oUser != null)
                {
                    if (this.model.PersonalDetails.Where(x => x.UserId == oUser.UserId).Any())
                    {
                        var oPerson = this.model.PersonalDetails.Where(x => x.UserId == oUser.UserId).FirstOrDefault();
                        oPerson.AddressLineOne = oModel.AddressOne;
                        oPerson.AddressLineTwo = oModel.AddressTwo;
                        oPerson.AlternateEmail = oModel.AlternateEmail;
                        oPerson.City = oModel.City;
                        oPerson.Country = oModel.Country;
                        oPerson.DateOfBirth = oModel.DateOfBirth;
                        oPerson.ExtentionCode = oModel.ExtentionCode;
                        if (oModel.Gender)
                        {
                            oPerson.Gender = "Male";
                        }
                        else
                        {
                            oPerson.Gender = "Female";
                        }
                        oPerson.MaritalStatus = oModel.Marital;
                        oPerson.MobileNumber = oModel.MobileNumber;
                        oPerson.PhoneNumber = oModel.PhoneNumber;
                        oPerson.PostalCode = oModel.PostalCode;
                        oPerson.State = oModel.State;
                        oPerson.Name = oModel.PersonName;
                    }
                    else
                    {
                        PersonalDetail oPerson = new PersonalDetail();
                        oPerson.AddressLineOne = oModel.AddressOne;
                        oPerson.AddressLineTwo = oModel.AddressTwo;
                        oPerson.AlternateEmail = oModel.AlternateEmail;
                        oPerson.City = oModel.City;
                        oPerson.Country = oModel.Country;
                        oPerson.DateOfBirth = oModel.DateOfBirth;
                        oPerson.ExtentionCode = oModel.ExtentionCode;
                        if (oModel.Gender)
                        {
                            oPerson.Gender = "Male";
                        }
                        else
                        {
                            oPerson.Gender = "Female";
                        }
                        oPerson.MaritalStatus = oModel.Marital;
                        oPerson.MobileNumber = oModel.MobileNumber;
                        oPerson.PhoneNumber = oModel.PhoneNumber;
                        oPerson.PostalCode = oModel.PostalCode;
                        oPerson.State = oModel.State;
                        oPerson.Name = oModel.PersonName;
                        oPerson.UserId = oUser.UserId;
                        this.model.PersonalDetails.Add(oPerson);
                    }
                }
                this.model.SaveChanges();
            }
            return View(oModel);
        }

        [HttpGet]
        public ActionResult Education()
        {
            ModelEducation oEducation = new ModelEducation();
            var oUser = this.model.UserProfiles.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (oUser != null)
            {
                var objEducation = this.model.Education.Where(x => x.UserId == oUser.UserId).FirstOrDefault();
                if (objEducation != null)
                {
                    oEducation.Certification = objEducation.Certification;
                    oEducation.Doctorate = objEducation.Doctorate;
                    oEducation.DoctorateEducationMode = objEducation.DoctorateEducationMode;
                    oEducation.DoctorateSpecialization = objEducation.DoctorateSpecialization;
                    oEducation.DoctorateUniversity = objEducation.DoctorateUniversity;
                    oEducation.DoctorateYearComplete = objEducation.DoctorateYearComplete;
                    oEducation.EducationMode = objEducation.EducationMode;
                    oEducation.Grade = objEducation.Grade;
                    oEducation.Graduation = objEducation.Graduation;
                    oEducation.PostGraduation = objEducation.PostGraduation;
                    oEducation.PostGraduationEducationMode = objEducation.PostEducationMode;
                    oEducation.PostSpecialization = objEducation.PostSpecialization;
                    oEducation.PostUniversity = objEducation.PostUnversity;
                    oEducation.PostYearComplete = objEducation.PostYearComplete;
                    oEducation.Specialization = objEducation.Specialization;
                    oEducation.University = objEducation.University;
                    oEducation.YearComplete = objEducation.YearComplete;
                }
            }
            return View(oEducation);
        }

        [HttpPost]
        public ActionResult Education(ModelEducation objEducation)
        {
            var oUser = this.model.UserProfiles.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (oUser != null)
            {
                var modelEducation = this.model.Education.Where(x => x.UserId == oUser.UserId).FirstOrDefault();
                if(modelEducation!=null)
                {
                    modelEducation.Certification = objEducation.Certification;
                    modelEducation.Doctorate = objEducation.Doctorate;
                    modelEducation.DoctorateEducationMode = objEducation.DoctorateEducationMode;
                    modelEducation.DoctorateSpecialization = objEducation.DoctorateSpecialization;
                    modelEducation.DoctorateUniversity = objEducation.DoctorateUniversity;
                    modelEducation.DoctorateYearComplete = objEducation.DoctorateYearComplete;
                    modelEducation.EducationMode = objEducation.EducationMode;
                    modelEducation.Grade = objEducation.Grade;
                    modelEducation.Graduation = objEducation.Graduation;
                    modelEducation.PostGraduation = objEducation.PostGraduation;
                    modelEducation.PostEducationMode = objEducation.PostGraduationEducationMode;
                    modelEducation.PostSpecialization = objEducation.PostSpecialization;
                    modelEducation.PostUnversity = objEducation.PostUniversity;
                    modelEducation.PostYearComplete = objEducation.PostYearComplete;
                    modelEducation.Specialization = objEducation.Specialization;
                    modelEducation.University = objEducation.University;
                    modelEducation.YearComplete = objEducation.YearComplete;
                }
                else
                {
                    MentorMe.Models.Education oEducation = new Education();
                    oEducation.Certification = objEducation.Certification;
                    oEducation.Doctorate = objEducation.Doctorate;
                    oEducation.DoctorateEducationMode = objEducation.DoctorateEducationMode;
                    oEducation.DoctorateSpecialization = objEducation.DoctorateSpecialization;
                    oEducation.DoctorateUniversity = objEducation.DoctorateUniversity;
                    oEducation.DoctorateYearComplete = objEducation.DoctorateYearComplete;
                    oEducation.EducationMode = objEducation.EducationMode;
                    oEducation.Grade = objEducation.Grade;
                    oEducation.Graduation = objEducation.Graduation;
                    oEducation.PostGraduation = objEducation.PostGraduation;
                    oEducation.PostEducationMode = objEducation.PostGraduationEducationMode;
                    oEducation.PostSpecialization = objEducation.PostSpecialization;
                    oEducation.PostUnversity = objEducation.PostUniversity;
                    oEducation.PostYearComplete = objEducation.PostYearComplete;
                    oEducation.Specialization = objEducation.Specialization;
                    oEducation.University = objEducation.University;
                    oEducation.YearComplete = objEducation.YearComplete;
                    oEducation.UserId = oUser.UserId;
                    this.model.Education.Add(oEducation);                    
                }
                this.model.SaveChanges();
            }
            return View(objEducation);
        }

        [HttpGet]
        public ActionResult Interest()
        {
            ModelAreaOfInterest oModel = new ModelAreaOfInterest();
            var oUser = this.model.UserProfiles.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (oUser != null)
            {
                var oInterest = this.model.AreaOfIntrests.Where(x => x.UserId == oUser.UserId).FirstOrDefault();
                if(oInterest!=null)
                {
                    oModel.Experiance = oInterest.Experience;
                    oModel.KeySkill = oInterest.KeySkills;
                }
            }
            return View(oModel);
        }

        [HttpPost]
        public ActionResult Interest(ModelAreaOfInterest oModel)
        {
            var oUser = this.model.UserProfiles.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (oUser != null)
            {
                var oInterest = this.model.AreaOfIntrests.Where(x => x.UserId == oUser.UserId).FirstOrDefault();
                if(oInterest!=null)
                {
                    oInterest.KeySkills = oModel.KeySkill;
                    oInterest.Experience = oModel.Experiance;
                }
                else
                {
                    AreaOfIntrest objectAreaOfInterest = new AreaOfIntrest();
                    objectAreaOfInterest.Experience = oModel.Experiance;
                    objectAreaOfInterest.KeySkills = oModel.KeySkill;
                    objectAreaOfInterest.UserId = oUser.UserId;
                    this.model.AreaOfIntrests.Add(objectAreaOfInterest);
                }
                this.model.SaveChanges();
            }
            return View(oModel);
        }
    }
}
