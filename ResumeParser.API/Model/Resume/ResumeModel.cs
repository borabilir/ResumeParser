using System;
using System.Collections.Generic;

namespace ResumeParser.API.Model
{
    public class ResumeModel
    {
        public ResumeModel()
        {
            PersonalInfo = new PersonalInfo();
            Skills = new List<Skill>();
            Languages = new List<Language>();
            Experiences = new List<Experience>();
            Educations = new List<Education>();
            Certificates = new List<Certificate>();
        }

        public PersonalInfo PersonalInfo { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Language> Languages { get; set; }
        public List<Experience> Experiences { get; set; }
        public List<Education> Educations { get; set; }
        public List<Certificate> Certificates { get; set; }
    }

    public abstract class ResumeInfo
    {

    }

    public class PersonalInfo : ResumeInfo
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Summary { get; set; }
        public string LinkedinProfile { get; set; }
    }

    public class Skill : ResumeInfo
    {
        public string Name { get; set; }
        public string Level { get; set; }
    }

    public class Language : ResumeInfo
    {
        public string Name { get; set; }
        public string Level { get; set; }
    }

    public class Experience : ResumeInfo
    {
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
    }

    public class Education : ResumeInfo
    {
        public string SchoolName { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public string SchoolLocation { get; set; }
        public string SchoolStartDate { get; set; }
        public string SchoolEndDate { get; set; }
    }

    public class Certificate : ResumeInfo
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}
