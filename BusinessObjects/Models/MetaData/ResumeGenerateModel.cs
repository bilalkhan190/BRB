using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class ResumeGenerateModel
    {
        public UserProfile UserProfile { get; set; } = new UserProfile();
        public Resume Resume { get; set; } = new Resume();
        public ContactInfo Contact { get; set; } = new ContactInfo();
        public ObjectiveSummary ObjectiveSummary { get; set; } = new ObjectiveSummary();
        public Education Education { get; set; } = new Education();
        public List<College> Colleges { get; set; } = new List<College>();
        public List<AcademicHonor> AcademicHonors { get; set; } = new List<AcademicHonor>();
        public List<AcademicScholarship> AcademicScholarships { get; set; } = new List<AcademicScholarship>();
        public OverseasExperience OverseasExperience { get; set; } = new OverseasExperience();
       public List<OverseasStudy> OverseasStudies { get; set; } = new List<OverseasStudy>();
        public MilitaryExperience MilitaryExperiences { get; set; } = new MilitaryExperience();
       public List<MilitaryPosition> MilitaryPositions { get; set; } = new List<MilitaryPosition>();
        public OrgExperience OrgExperience { get; set; } = new OrgExperience();
        public List<Organization> Organizations { get; set; } = new List<Organization>();
        public List<OrgPosition> OrgPositions { get; set; } = new List<OrgPosition>();
        public VolunteerExperience VolunteerExperience { get; set; } = new VolunteerExperience();
        public List<VolunteerOrg> VolunteerOrgs { get; set; } = new List<VolunteerOrg>();
        public List<VolunteerPosition> VolunteerPositions { get; set; } = new List<VolunteerPosition>();
        public Professional Professional { get; set; } = new Professional();
        public List<License> Licenses { get; set; } = new List<License>();
        public List<Certificate> Certificates { get; set; } = new List<Certificate>();
        public List<Affiliation> Affiliations { get; set; } = new List<Affiliation>();
        public List<AffiliationPosition> AffiliationPositions { get; set; } = new List<AffiliationPosition>();
        public TechnicalSkill TechnicalSkill { get; set; } = new TechnicalSkill();

        public LanguageSkill LanguageSkill { get; set; } = new LanguageSkill();
        public List<Language> Languages { get; set; } = new List<Language>();

    }
}
