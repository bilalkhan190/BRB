using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BusinessObjects.APIModels;
using BusinessObjects.Models;
using ContactInfo = BusinessObjects.Models.ContactInfo;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models.MetaData;
using System.Globalization;
using DocumentFormat.OpenXml.Office.Word;

namespace BRB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        protected readonly Wh4lprodContext _dbContext;

        public MigrationController()
        {
            _dbContext = new Wh4lprodContext();
        }

        [HttpGet]
        [Route("Migrate")]
        public IActionResult Migrate(string keyword)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Data not found. Please enter email address, Phone number or User ID";
            var UserRecord = _dbContext.UserProfiles.FirstOrDefault(x => x.UserName.ToLower() == keyword.ToLower() || x.UserId.ToString() == keyword || x.Phone == keyword);
            if(UserRecord != null)
            {
                var ResumeData = _dbContext.Resumes.FirstOrDefault(x => x.UserId == UserRecord.UserId);
               
                if (ResumeData != null)
                {
                    var data = JsonConvert.DeserializeObject<Root>(ResumeData.Data);
                    if (data != null)
                    {
                        ObjectiveSummary objectiveSummary = new ObjectiveSummary
                        {
                            ObjectiveType = data.objective.summaryChoice,
                            ResumeId = ResumeData.ResumeId,
                            YearsOfExperienceDesc = data.objective.experience,
                            YearsOfExperienceId = _dbContext.YearsOfExperienceLists.FirstOrDefault(x => x.YearsOfExperienceDesc == data.objective.experience).YearsOfExperienceId,
                            PositionTypeDesc = data.objective.positionType,
                            PositionTypeId = _dbContext.PositionTypeLists
                           .FirstOrDefault(x => x.PositionTypeDesc.Contains(data.objective.positionType)).PositionTypeId,
                            PositionTypeOther = data.objective.otherPositionType,
                            CurrentCompanyType = data.objective.companyOrIndustry,
                            ChangeTypeDesc = data.objective.positiveChange,
                            ChangeTypeId = _dbContext.ChangeTypeLists.FirstOrDefault(x => x.ChangeTypeDesc.Contains(data.objective.positiveChange)).ChangeTypeId,
                            ChangeTypeOther = data.objective.otherPositiveChange,
                            IsComplete = data.objective.sectionComplete
                            
                        };
                        if (data.objective.skills.Count > 2)
                        {
                            objectiveSummary.ObjectiveDesc1 = data.objective.skills[0];
                            objectiveSummary.Objective1Id = data.objective.skills[0] != "" ? _dbContext.ObjectiveLists
                            .FirstOrDefault(x => x.ObjectiveDesc.Contains(data.objective.skills[0])).ObjectiveId : 0;
                            objectiveSummary.ObjectiveDesc2 = data.objective.skills[1];
                            objectiveSummary.Objective2Id = data.objective.skills[1] != "" ? _dbContext.ObjectiveLists
                           .FirstOrDefault(x => x.ObjectiveDesc.Contains(data.objective.skills[1])).ObjectiveId : 0;
                            objectiveSummary.ObjectiveDesc3 = data.objective.skills[2];
                            objectiveSummary.Objective3Id = data.objective.skills[2] != "" ? _dbContext.ObjectiveLists
                           .FirstOrDefault(x => x.ObjectiveDesc.Contains(data.objective.skills[2])).ObjectiveId : 0;
                        }
                        else
                        {
                            objectiveSummary.ObjectiveDesc1 = data.objective.skills[0];
                            objectiveSummary.Objective1Id = data.objective.skills[0] != "" ? _dbContext.ObjectiveLists
                            .FirstOrDefault(x => x.ObjectiveDesc.Contains(data.objective.skills[0])).ObjectiveId : 0;
                            objectiveSummary.ObjectiveDesc2 = data.objective.skills[1];
                            objectiveSummary.Objective2Id = data.objective.skills[1] != "" ? _dbContext.ObjectiveLists
                           .FirstOrDefault(x => x.ObjectiveDesc.Contains(data.objective.skills[1])).ObjectiveId : 0;
                        }
                        _dbContext.ObjectiveSummaries.Add(objectiveSummary);
                        _dbContext.SaveChanges();
                        _dbContext.ContactInfos.Add(new ContactInfo
                        {
                            FirstName = data.contactInfo.firstName,
                            LastName = data.contactInfo.lastName,
                            Address1 = data.contactInfo.address1,
                            Address2 = data.contactInfo.address2,
                            City = data.contactInfo.city,
                            StateAbbr = _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(data.contactInfo.state)).StateAbbr,
                            ZipCode = data.contactInfo.zipcode.ToString(),
                            Phone = data.contactInfo.phone,
                            Email = data.contactInfo.email,
                            CreatedDate = DateTime.Today,
                            IsComplete = data.contactInfo.sectionComplete,
                            ResumeId = ResumeData.ResumeId,

                        });
                        _dbContext.SaveChanges();
                        BusinessObjects.Models.Education educationMaster = new BusinessObjects.Models.Education
                        {
                            ResumeId = ResumeData.ResumeId,
                            CreatedDate = DateTime.Today,
                            IsComplete = data.education.sectionComplete,
                        };
                        _dbContext.Educations.Add(educationMaster);
                        _dbContext.SaveChanges();

                        List<College> colleges = new List<College>();
                        List<AcademicHonor> academicHonors = new List<AcademicHonor>();
                        List<AcademicScholarship> academicScholarships = new List<AcademicScholarship>();
                        foreach (var education in data.education.schools)
                        {
                            College college = new College();
                            college.CollegeName = education.name;
                            college.CollegeCity = education.city;
                            college.EducationId = educationMaster.EducationId;
                            college.CollegeStateAbbr = _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(education.state)).StateAbbr;
                            college.GradDate = new DateTime(DateTime.ParseExact(education.graduationYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(education.graduationMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1);
                            college.Month = education.graduationMonth;
                            college.Year = education.graduationYear;
                            college.SchoolName = education.schoolOrProgramName;
                            college.DegreeDesc = education.degreeType;
                            college.DegreeId = _dbContext.DegreeLists.FirstOrDefault(x => x.DegreeDesc.Contains(education.degreeType)).DegreeId;
                            college.MajorDesc = education.major;
                            college.MajorId = _dbContext.MajorLists.FirstOrDefault(x => x.MajorDesc.Contains(education.major)).MajorId;
                            college.MajorSpecialtyDesc = education.majorSpecialty;
                            college.MajorSpecialtyId = _dbContext.MajorSpecialtyLists.FirstOrDefault(x => x.MajorSpecialtyDesc.Contains(education.majorSpecialty)).MajorSpecialtyId;
                            college.MinorDesc = education.minor;
                            college.MinorId = _dbContext.MinorLists.FirstOrDefault(x => x.MinorDesc.Contains(education.minor)).MinorId;
                            college.CertificateDesc = education.eduCert;
                            college.CertificateId = _dbContext.CertificateLists.FirstOrDefault(x => x.CertificateDesc.Contains(education.eduCert.Trim()))?.CertificateId;
                            college.IncludeGpa = education.includeGpa;
                            college.CreatedDate = DateTime.Today;
                            college.LastModDate = DateTime.Today;
                            college.HonorProgram = education.honorsCollegeProgram;
                            college.IncludeGpa = education.includeGpa;
                            college.Gpa = education.gpa;
                            _dbContext.Colleges.Add(college);
                            _dbContext.SaveChanges();
                            foreach (var honor in education.honors)
                            {
                                AcademicHonor academicHonor = new AcademicHonor
                                {
                                    CollegeId = college.CollegeId,
                                    HonorName = honor.honor,
                                    HonorMonth = honor.month,
                                    HonorYear = honor.year,
                                    CreatedDate = DateTime.Now,
                                    LastModDate = DateTime.Now
                                };
                                academicHonors.Add(academicHonor);
                            }
                            foreach (var schlr in education.scholoarships)
                            {
                                AcademicScholarship academicScholarship = new AcademicScholarship
                                {
                                    CollegeId = college.CollegeId,
                                    ScholarshipName = schlr.name,
                                    ScholarshipCriteria = schlr.criteria,
                                    ScholarshipMonth = schlr.month,
                                    ScholarshipYear = schlr.year,
                                    CreatedDate = DateTime.Now,
                                    LastModDate = DateTime.Now
                                };
                                academicScholarships.Add(academicScholarship);
                            }

                            //colleges.Add(college);

                        }

                        _dbContext.AcademicHonors.AddRange(academicHonors);
                        _dbContext.AcademicScholarships.AddRange(academicScholarships);
                        _dbContext.SaveChanges();
                        OverseasExperience overseasExperience = new OverseasExperience
                        {
                            ResumeId = ResumeData.ResumeId,
                            CreatedDate = DateTime.Now,
                            IsComplete = data.overseasStudies.sectionComplete,
                            IsOptOut = data.overseasStudies.doesNotApply,
                            LastModDate = DateTime.Now,
                        };
                        _dbContext.OverseasExperiences.Add(overseasExperience);
                        _dbContext.SaveChanges();
                        List<OverseasStudy> overseasStudies = new List<OverseasStudy>();
                        foreach (var ov in data.overseasStudies.overseasStudies)
                        {
                            OverseasStudy overseasStudy = new OverseasStudy
                            {
                                CollegeName = ov.college,
                                OverseasExperienceId = overseasExperience.OverseasExperienceId,
                                City = ov.city,
                                StartedDate = ov.startMonth != null && ov.startYear != null ? new DateTime(DateTime.ParseExact(ov.startYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(ov.startMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1):null,
                                EndedDate = ov.endMonth != null && ov.endYear != null ?new DateTime(DateTime.ParseExact(ov.endYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(ov.endMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1):null,
                                CountryName = data.overseasStudies.country,
                                CountryId = ov.country != null ? _dbContext.CountryLists.FirstOrDefault(x => x.CountryName == ov.country)?.CountryId : 1,
                                ClassesCompleted = ov.mainClasses,
                                LivingSituationId = ov.otherLivingSit != null ? _dbContext.LivingSituationLists.FirstOrDefault(x => x.LivingSituationDesc.Contains(ov.livingSit))?.LivingSituationId : 1,
                                OtherInfo = ov.otherInfo,
                            };
                            overseasStudies.Add(overseasStudy);
                        }
                        _dbContext.OverseasStudies.AddRange(overseasStudies);
                        _dbContext.SaveChanges();

                        BusinessObjects.Models.MilitaryExperience militaryExperience = new BusinessObjects.Models.MilitaryExperience
                        {
                            Branch = data.militaryExperience.militaryBranch,
                            ResumeId = ResumeData.ResumeId,
                            City = data.militaryExperience.primaryCityStationed,
                            CountryId = data.militaryExperience.countryStationed != null ? _dbContext.CountryLists.FirstOrDefault(x => x.CountryName.Contains(data.militaryExperience.countryStationed)).CountryId : 0,
                            StartedMonth = data.militaryExperience.startMonth,
                            StartedYear = data.militaryExperience.startYear,
                            EndedMonth = data.militaryExperience.endMonth,
                            EndedYear = data.militaryExperience.endYear,
                            Rank = data.militaryExperience.rank,
                            IsComplete = data.militaryExperience.sectionComplete
                        };
                        _dbContext.MilitaryExperiences.Add(militaryExperience);
                        _dbContext.SaveChanges();
                        List<MilitaryPosition> militaryPositions = new List<MilitaryPosition>();
                        foreach (var position in data.militaryExperience.positions)
                        {
                            MilitaryPosition milPosition = new MilitaryPosition
                            {
                                MilitaryExperienceId = militaryExperience.MilitaryExperienceId,
                                Title = position.title,
                                StartedMonth = position.startMonth,
                                StartedYear = position.startYear,
                                EndedMonth = position.endMonth,
                                EndedYear = position.endYear,
                                OtherInfo = position.otherInfo,
                                MainTraining = position.specialTraining,
                                Responsibility1 = position.responsibilities.responsibility1,
                                Responsibility2 = position.responsibilities.responsibility2,
                                Responsibility3 = position.responsibilities.responsibility3,
                            };
                            militaryPositions.Add(milPosition);
                        }
                        _dbContext.MilitaryPositions.AddRange(militaryPositions);
                        _dbContext.SaveChanges();

                        OrgExperience orgExperience = new OrgExperience
                        {
                            CreatedDate = DateTime.Now,
                            IsComplete = data.orgs.sectionComplete,
                            ResumeId = ResumeData.ResumeId,
                            IsOptOut = data.orgs.doesNotApply
                        };
                        _dbContext.OrgExperiences.Add(orgExperience);
                        _dbContext.SaveChanges();
                        List<Organization> organizations = new List<Organization>();
                        List<OrgPosition> orgPositions = new List<OrgPosition>();
                        foreach (var org in data.orgs.orgs)
                        {
                            Organization organization = new Organization
                            {
                                OrgName = org.name,
                                City = org.city,
                                StateAbbr = org.state != null ? _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(org.state)).StateAbbr : "",
                                StartedMonth = org.startMonth,
                                StartedYear = org.startYear,
                                CreatedDate = DateTime.Now,
                                OrgExperienceId = orgExperience.OrgExperienceId,


                            };

                            if (org.currentlyIn)
                            {
                                organization.EndedMonth = "";
                                organization.EndedYear = "";
                            }
                            else
                            {
                                organization.EndedMonth = org.endMonth;
                                organization.EndedYear = org.endYear;
                            }
                            _dbContext.Organizations.Add(organization);
                            _dbContext.SaveChanges();
                            //organizations.Add(organization);
                            foreach (var pos in org.positions)
                            {
                                OrgPosition orgPos = new OrgPosition
                                {
                                    Title = pos.title,
                                    StartedMonth = pos.startMonth,
                                    StartedYear = pos.startYear,
                                    OtherInfo = pos.otherInfo,
                                    Responsibility1 = pos.responsibilities.responsibility1,
                                    Responsibility2 = pos.responsibilities.responsibility2,
                                    Responsibility3 = pos.responsibilities.responsibility3,
                                    OrganizationId = organization.OrganizationId,
                                };
                                if (pos.currentlyIn)
                                {
                                    orgPos.EndedMonth = "";
                                    orgPos.EndedYear = "";
                                }
                                else
                                {
                                    orgPos.EndedMonth = pos.endMonth;
                                    orgPos.EndedYear = pos.endYear;
                                }
                                orgPositions.Add(orgPos);
                            }

                        }


                        _dbContext.OrgPositions.AddRange(orgPositions);
                        _dbContext.SaveChanges();


                        VolunteerExperience volunteer = new VolunteerExperience
                        {
                            CreatedDate = DateTime.Now,
                            IsComplete = data.communityServices.sectionComplete,
                            ResumeId = ResumeData.ResumeId,
                            IsOptOut = data.communityServices.doesNotApply
                        };
                        _dbContext.VolunteerExperiences.Add(volunteer);
                        _dbContext.SaveChanges();
                        List<VolunteerOrg> volunteerOrgs = new List<VolunteerOrg>();
                        List<VolunteerPosition> volunteerPositions = new List<VolunteerPosition>();
                        foreach (var org in data.communityServices.services)
                        {
                            VolunteerOrg Volorganization = new VolunteerOrg
                            {
                                VolunteerOrg1 = org.name,
                                City = org.city,
                                StateAbbr = _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(org.state)).StateAbbr,
                                StartedMonth = org.startMonth,
                                StartedYear = org.startYear,
                                CreatedDate = DateTime.Now,
                                VolunteerExperienceId = volunteer.VolunteerExperienceId,
                            };
                            if (org.currentlyIn)
                            {
                                Volorganization.EndedMonth = null;
                                Volorganization.EndedYear = null;
                            }
                            else
                            {
                                Volorganization.EndedMonth = org.endMonth;
                                Volorganization.EndedYear = org.endYear;
                            }
                            //volunteerOrgs.Add(Volorganization);

                            _dbContext.VolunteerOrgs.Add(Volorganization);
                            _dbContext.SaveChanges();
                            foreach (var Orgpos in org.positions)
                            {
                                VolunteerPosition volPos = new VolunteerPosition
                                {
                                    Title = Orgpos.title,
                                    StartedMonth = Orgpos.startMonth,
                                    StartedYear = Orgpos.startYear,
                                    VolunteerOrgId = Volorganization.VolunteerOrgId,
                                    OtherInfo = Orgpos.otherInfo,
                                    Responsibility1 = Orgpos.responsibilities.responsibility1,
                                    Responsibility2 = Orgpos.responsibilities.responsibility2,
                                    Responsibility3 = Orgpos.responsibilities.responsibility3,
                                };
                                if (Orgpos.currentlyIn)
                                {
                                    volPos.EndedMonth = null;
                                    volPos.EndedYear = null;
                                }
                                else
                                {
                                    volPos.EndedMonth = Orgpos.endMonth;
                                    volPos.EndedYear = Orgpos.endYear;
                                }
                                volunteerPositions.Add(volPos);
                            }

                        }

                        _dbContext.VolunteerPositions.AddRange(volunteerPositions);
                        _dbContext.SaveChanges();
                        LanguageSkill languageSkill = new LanguageSkill()
                        {
                            IsComplete = data.languages.sectionComplete,
                            IsOptOut = data.languages.doesNotApply,
                            CreatedDate = DateTime.Today,
                            ResumeId = ResumeData.ResumeId
                        };
                        _dbContext.LanguageSkills.Add(languageSkill);
                        _dbContext.SaveChanges();
                        List<Language> languageSkills = new List<Language>();
                        foreach (var lang in data.languages.langauges)
                        {
                            BusinessObjects.Models.Language langauge = new BusinessObjects.Models.Language
                            {
                                LanguageName = lang.language,
                                Ability = lang.ability,
                                LanguageAbilityId = lang.ability != null ? _dbContext.LanguageAbilityLists.FirstOrDefault(x => x.LanguageAbilityDesc.Contains(lang.ability)).LanguageAbilityId : 0,
                                CreatedDate = DateTime.Today,
                                LanguageSkillId = languageSkill.LanguageSkillId,
                            };
                            languageSkills.Add(langauge);
                        }
                        _dbContext.Languages.AddRange(languageSkills);
                        _dbContext.SaveChanges();

                        Professional professional = new Professional
                        {
                            IsComplete = data.lcas.sectionComplete,
                            IsOptOut = data.lcas.doesNotApply,
                            CreatedDate = DateTime.Today,
                            ResumeId = ResumeData.ResumeId,
                        };
                        _dbContext.Professionals.Add(professional);
                        _dbContext.SaveChanges();
                        List<License> licenses = new List<License>();
                        foreach (var license in data.lcas.liscenses)
                        {
                            License lcs = new License
                            {
                                Title = license.title,
                                StateAbbr = _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(license.state)).StateAbbr,
                                CreatedDate = DateTime.Now,
                                ReceivedMonth = license.month,
                                ReceivedYear = license.year,
                                ProfessionalId = professional.ProfessionalId
                            };
                            licenses.Add(lcs);
                        }
                        _dbContext.Licenses.AddRange(licenses);
                        _dbContext.SaveChanges();
                        List<Certificate> certificates = new List<Certificate>();
                        foreach (var c in data.lcas.certs)
                        {
                            Certificate cert = new Certificate
                            {
                                Title = c.title,
                                StateAbbr = _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(c.state)).StateAbbr,
                                CreatedDate = DateTime.Now,
                                ReceivedMonth = c.month,
                                ReceivedYear = c.year,
                                ProfessionalId = professional.ProfessionalId
                            };
                            certificates.Add(cert);
                        }
                        _dbContext.Certificates.AddRange(certificates);
                        _dbContext.SaveChanges();
                        List<Affiliation> affiliations = new List<Affiliation>();
                        List<AffiliationPosition> affiliationPositions = new List<AffiliationPosition>();
                        foreach (var aff in data.lcas.orgs)
                        {
                            Affiliation affiliation = new Affiliation();

                            affiliation.AffiliationName = aff.title;
                            affiliation.City = aff.city;
                            affiliation.StateAbbr = _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(aff.state)).StateAbbr;
                            affiliation.CreatedDate = DateTime.Now;
                            affiliation.LastModDate = DateTime.Now;
                            affiliation.StartedMonth = aff.startMonth;
                            affiliation.StartedYear = aff.startYear;
                            affiliation.EndedMonth = aff.endMonth;
                            affiliation.EndedYear = aff.endYear;
                            affiliation.ProfessionalId = professional.ProfessionalId;
                            _dbContext.Affiliations.Add(affiliation);
                            _dbContext.SaveChanges();
                            //affiliations.Add(affiliation);

                            foreach (var p in aff.positions)
                            {
                                AffiliationPosition affiliationPosition = new AffiliationPosition
                                {
                                    Title = p.title,
                                    StartedMonth = p.startMonth,
                                    StartedYear = p.startYear,
                                    EndedMonth = p.endMonth,
                                    EndedYear = p.endYear,
                                    CreatedDate = DateTime.Now,
                                    OtherInfo = p.otherInfo,
                                    Responsibility1 = p.responsibilities.responsibility1,
                                    Responsibility2 = p.responsibilities.responsibility2,
                                    Responsibility3 = p.responsibilities.responsibility3,
                                    AffiliationId = affiliation.AffiliationId
                                };
                                //affiliationPositions.Add(affiliationPosition);
                                _dbContext.AffiliationPositions.Add(affiliationPosition);
                            }
                            _dbContext.SaveChanges();

                        }
                        
                        BusinessObjects.Models.WorkExperience workExperience = new BusinessObjects.Models.WorkExperience
                        {
                            ResumeId = ResumeData.ResumeId,
                            IsComplete = data.workExperience.sectionComplete,
                            CreatedDate = DateTime.Now,
                        };

                        _dbContext.WorkExperiences.Add(workExperience);
                        _dbContext.SaveChanges();
                        List<WorkCompany> workCompanyList = new List<WorkCompany>();
                        List<ResponsibilityOptionsResponse> responsibilityOptionsResponseList = new List<ResponsibilityOptionsResponse>();
                        List<WorkRespQuestion> WorkRespQuestionList = new List<WorkRespQuestion>();
                        List<WorkPosition> workPositionList = new List<WorkPosition>();
                        List<JobAward> jobAwardList = new List<JobAward>();
                        WorkPosition workPosition = null;
                        foreach (var c in data.workExperience.copanies)
                        {
                            WorkCompany workCompany = new WorkCompany
                            {
                                CompanyName = c.name,
                                State = c.state != null ? _dbContext.StateLists.FirstOrDefault(x => x.StateName.Contains(c.state)).StateAbbr : "",
                                WorkExperienceId = workExperience.WorkExperienceId,
                                StartMonth = c.startMonth != null ? c.startMonth : null,
                                StartYear = c.startYear != null ? Convert.ToInt16(c.startYear):null,
                                EndMonth = c.endMonth != null ? c.endMonth : null,
                                EndYear = c.endYear != null? Convert.ToInt32(c.endYear) : null,
                                City = c.city,
                            };
                            if (c.currentlyIn)
                            {
                                workCompany.EndMonth = null;
                                workCompany.EndYear = null;
                            }
                            else
                            {
                                workCompany.EndMonth = c.endMonth;
                                workCompany.EndYear = Convert.ToInt16(c.endYear);
                            }
                            _dbContext.WorkCompanies.Add(workCompany);
                            _dbContext.SaveChanges();
                            foreach (var j in c.jobs)
                            {
                                workPosition = new WorkPosition
                                {
                                    CompanyId = workCompany.CompanyId,
                                    JobResponsibilityId = j.responsibility != null ? _dbContext.JobCategoryLists.FirstOrDefault(x => x.JobCategoryDesc.Contains(j.responsibility)).JobCategoryId : 0,
                                    Title = j.title,
                                    StartMonth = j.startMonth,
                                    StartYear = Convert.ToInt16(j.startYear),
                                    EndMonth = j.endMonth != null ? j.endMonth : null,
                                    EndYear = j.endYear != null ? Convert.ToInt16(j.endYear) : null,
                                    Project1 = j.project1,
                                    Project2 = j.project2,
                                    ImproveProductivity = j.processImprovement,
                                    PercentageImprovement = j.percentProductivityImprove,
                                    IncreaseRevenue = j.revenueIncrease,
                                };
                                _dbContext.WorkPositions.Add(workPosition);
                                _dbContext.SaveChanges();
                                foreach (var opt in j.respOptions)
                                {
                                    ResponsibilityOptionsResponse responsibilityOptionsResponse = new ResponsibilityOptionsResponse
                                    {
                                        Caption = opt,
                                        PositionId = workPosition.PositionId,
                                        ResponsibilityOption = opt != null ? _dbContext.ResponsibilityOptions.FirstOrDefault(x => x.Caption == opt).RespOptionId : 0,
                                    };
                                    _dbContext.ResponsibilityOptionsResponses.Add(responsibilityOptionsResponse);
                                    _dbContext.SaveChanges();
                                }
                               
                                foreach (var faq in j.specificJobAnswers)
                                {
                                    WorkRespQuestion workRespQuestion = new WorkRespQuestion
                                    {
                                        PositionId = workPosition.PositionId,
                                        RespQuestionId = faq.question.question != null ? _dbContext.ResponsibilityQuestions.FirstOrDefault(x => x.Caption == faq.question.question).RespQuestionId : 0,
                                        Question = faq.question.question != null ? faq.question.question : "",
                                        Answer = faq.answer == null ? "" : faq.answer.ToString(),
                                    };
                                    _dbContext.WorkRespQuestions.Add(workRespQuestion);

                                }
                                _dbContext.SaveChanges();
                                foreach (var award in j.awards)
                                {
                                    JobAward jobAward = new JobAward
                                    {
                                        AwardDesc = award.name,
                                        AwardedMonth = award.month,
                                        AwardedYear = award.year,
                                        CompanyJobId = workPosition.PositionId,
                                    };
                                    _dbContext.JobAwards.Add(jobAward);
                                }
                                _dbContext.SaveChanges();

                            }
                        }
                        TechnicalSkill technicalSkills = new TechnicalSkill();
                        foreach (var mp in data.technicalSkills.microsoftPrograms)
                        {
                            switch (mp)
                            {
                                case "Microsoft Word": technicalSkills.Msword = true; break;
                                case "Microsoft Power Point": technicalSkills.MspowerPoint = true; break;
                                case "Microsoft Excel": technicalSkills.Msexcel = true; break;
                                case "Microsoft Outlook Express": technicalSkills.Msoutlook = true; break;
                                default:
                                   // "Microsoft Word": technicalSkills.Msword = true; break;
                                    break;
                            }
                        }
                        foreach (var mt in data.technicalSkills.macintoshPrograms)
                        {
                            switch (mt)
                            {
                                case "Macintosh Pages": technicalSkills.MacPages = true; break;
                                case "Macintosh Numbers": technicalSkills.MacNumbers = true; break;
                                case "Macintosh Keynote": technicalSkills.MacKeynote = true; break;
                                default:
                                    //"Microsoft Word": technicalSkills.Msword = true; break;
                                    break;
                            }
                        }
                        foreach (var mt in data.technicalSkills.otherPrograms)
                        {
                            switch (mt)
                            {
                                case "Adobe Acrobat": technicalSkills.AdobeAcrobat = true; break;
                                case "Adobe Publisher": technicalSkills.AdobePublisher = true; break;
                                case "Adobe Illustrator": technicalSkills.AdobeIllustrator = true; break;
                                case "Adobe Photoshop": technicalSkills.AdobePhotoshop = true; break;
                                case "Google Suite": technicalSkills.GoogleSuite = true; break;
                                case "Google Docs": technicalSkills.GoogleDocs = true; break;
                                default:
                                    //"Microsoft Word": technicalSkills.Msword = true; break;
                                    break;
                            }
                        }
                        technicalSkills.IsComplete = data.technicalSkills.sectionComplete;
                        technicalSkills.OtherProgram = data.technicalSkills.otherProgramText;
                        technicalSkills.ResumeId = ResumeData.ResumeId;
                        _dbContext.TechnicalSkills.Add(technicalSkills);
                        _dbContext.SaveChanges();
                        ajaxResponse.Message = "record has been inserted";
                    }


                }
            }
            
            return Ok(ajaxResponse);
        }
        
    }
}
