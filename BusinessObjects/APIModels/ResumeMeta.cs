using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.APIModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Award
    {
        public string id { get; set; }
        public string name { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public bool naveValid { get; set; }
        public bool datesValid { get; set; }
    }

    public class Cert
    {
        public bool titleValid { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string state { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }

    public class CommunityServices
    {
        public List<Service> services { get; set; }
        public bool sectionComplete { get; set; }
        public bool doesNotApply { get; set; }
        public ServiceForPosition serviceForPosition { get; set; }
    }

    public class ContactInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int zipcode { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool sectionComplete { get; set; }
    }

    public class Copany
    {
        public string id { get; set; }
        public string name { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public bool currentlyIn { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public List<Job> jobs { get; set; }
        public bool nameValid { get; set; }
        public bool cityValid { get; set; }
        public bool datesValid { get; set; }
    }

    public class Education
    {
        public List<School> schools { get; set; }
        public bool sectionComplete { get; set; }
    }

    public class Honor
    {
        public string id { get; set; }
        public string honor { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public bool honorValid { get; set; }
        public bool datesValid { get; set; }
    }

    public class Job
    {
        public string id { get; set; }
        public string title { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public bool currentlyIn { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public string responsibility { get; set; }
        public List<string> respOptions { get; set; }
        public List<SpecificJobAnswer> specificJobAnswers { get; set; }
        public string project1 { get; set; }
        public string project2 { get; set; }
        public string processImprovement { get; set; }
        public int revenueIncrease { get; set; }
        public int percentProductivityImprove { get; set; }
        public List<Award> awards { get; set; }
        public bool titleValid { get; set; }
        public bool datesValid { get; set; }
        public bool otherResponsibilityValid { get; set; }
        public bool otherTopResponsibilitiesValid { get; set; }
        public bool otherRespClickValid { get; set; }
    }

    public class Langauge
    {
        public string id { get; set; }
        public int LanguageSkillId { get; set; }
        public string language { get; set; }
        public string ability { get; set; }
        public string LanguageName { get; set; }
        public int LanguageAbilityId { get; set; }
    }

    public class Languages
    {
        public List<Langauge> langauges { get; set; }
        public bool sectionComplete { get; set; }
        public bool doesNotApply { get; set; }
    }

    public class Lcas
    {
        public List<Liscense> liscenses { get; set; }
        public List<Cert> certs { get; set; }
        public List<Org> orgs { get; set; }
        public bool sectionComplete { get; set; }
        public bool doesNotApply { get; set; }
    }

    public class Liscense
    {
        public bool titleValid { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string state { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }

    public class MilitaryExperience
    {
        public string militaryBranch { get; set; }
        public string primaryCityStationed { get; set; }
        public string countryStationed { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public bool currentlyIn { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public string rank { get; set; }
        public List<Position> positions { get; set; }
        public bool branchValid { get; set; }
        public bool cityValid { get; set; }
        public bool rankValid { get; set; }
        public bool datesValid { get; set; }
        public bool sectionComplete { get; set; }
        public bool doesNotApply { get; set; }
    }

    public class Objective
    {
        public string summaryChoice { get; set; }
        public string experience { get; set; }
        public List<string> skills { get; set; }
        public string positionType { get; set; }
        public string otherPositionType { get; set; }
        public string companyOrIndustry { get; set; }
        public string positiveChange { get; set; }
        public string otherPositiveChange { get; set; }
        public bool sectionComplete { get; set; }
        public bool otherPositionTypeValid { get; set; }
        public bool otherPositiveChangeValid { get; set; }
    }

    public class Org
    {
        public bool nameValid { get; set; }
        public bool cityValid { get; set; }
        public bool datesValid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public bool currentlyIn { get; set; }
        public List<Position> positions { get; set; }
        public bool titleValid { get; set; }
        public string title { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public List<Org> orgs { get; set; }
        public bool sectionComplete { get; set; }
        public bool doesNotApply { get; set; }
        public string orgIdForPosition { get; set; }
    }

    public class OverseasStudies
    {
        public List<OverseasStudies> overseasStudies { get; set; }
        public bool sectionComplete { get; set; }
        public bool doesNotApply { get; set; }
        public string id { get; set; }
        public string college { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public bool currentlyIn { get; set; }
        public string mainClasses { get; set; }
        public string livingSit { get; set; }
        public string otherInfo { get; set; }
        public bool collegeValid { get; set; }
        public bool cityValid { get; set; }
        public bool datesValid { get; set; }
        public bool mainClassesValid { get; set; }
        public bool otherLivingValid { get; set; }
        public string otherLivingSit { get; set; }
    }

    public class Position
    {
        public string id { get; set; }
        public string title { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public bool currentlyIn { get; set; }
        public string otherInfo { get; set; }
        public string specialTraining { get; set; }
        public Responsibilities responsibilities { get; set; }
        public bool titleValid { get; set; }
        public bool datesValid { get; set; }
        public bool responsibiliesValid { get; set; }
        public bool specialTrainingValid { get; set; }
    }

    public class Question
    {
        public string question { get; set; }
        public string responseType { get; set; }
        public List<string> jobResponsibilities { get; set; }
        public int? digits { get; set; }
    }

    public class Responsibilities
    {
        public string responsibility1 { get; set; }
        public string responsibility2 { get; set; }
        public string responsibility3 { get; set; }
    }

    public class Root
    {
        public User user { get; set; }
        public ContactInfo contactInfo { get; set; }
        public Objective objective { get; set; }
        public Education education { get; set; }
        public OverseasStudies overseasStudies { get; set; }
        public WorkExperience workExperience { get; set; }
        public MilitaryExperience militaryExperience { get; set; }
        public Org orgs { get; set; }
        public CommunityServices communityServices { get; set; }
        public Lcas lcas { get; set; }
        public TechnicalSkills technicalSkills { get; set; }
        public Languages languages { get; set; }
    }

    public class Scholoarship
    {
        public string id { get; set; }
        public string name { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string criteria { get; set; }
        public bool nameValid { get; set; }
        public bool datesValid { get; set; }
        public bool criteriaValid { get; set; }
    }

    public class School
    {
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool currentlyAttending { get; set; }
        public string graduationMonth { get; set; }
        public string graduationYear { get; set; }
        public string schoolOrProgramName { get; set; }
        public string degreeType { get; set; }
        public string major { get; set; }
        public string majorSpecialty { get; set; }
        public string minor { get; set; }
        public string eduCert { get; set; }
        public string gpa { get; set; }
        public bool includeGpa { get; set; }
        public string honorsCollegeProgram { get; set; }
        public List<Honor> honors { get; set; }
        public List<Scholoarship> scholoarships { get; set; }
        public bool nameValid { get; set; }
        public bool cityValid { get; set; }
        public bool datesValid { get; set; }
        public bool otherDegreeValid { get; set; }
        public bool otherMajorValid { get; set; }
        public bool otherMajorSpecialtyValid { get; set; }
        public bool otherMinorValid { get; set; }
        public bool otherCertValid { get; set; }
    }

    public class Service
    {
        public bool nameValid { get; set; }
        public bool cityValid { get; set; }
        public bool datesValid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public bool currentlyIn { get; set; }
        public List<Position> positions { get; set; }
    }

    public class ServiceForPosition
    {
        public bool nameValid { get; set; }
        public bool cityValid { get; set; }
        public bool datesValid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public bool currentlyIn { get; set; }
        public List<object> positions { get; set; }
    }

    public class SpecificJobAnswer
    {
        public Question question { get; set; }
        public object answer { get; set; }
    }

    public class TechnicalSkills
    {
        public List<string> microsoftPrograms { get; set; }
        public List<string> macintoshPrograms { get; set; }
        public List<string> otherPrograms { get; set; }
        public string otherProgramText { get; set; }
        public bool sectionComplete { get; set; }
    }

    public class User
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string generatedFile { get; set; }
    }

    public class WorkExperience
    {
        public List<Copany> copanies { get; set; }
        public bool sectionComplete { get; set; }
    }


}
