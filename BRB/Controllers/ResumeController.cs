using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
//using BusinessObjects.Models.MetaData.New;
using BusinessObjects.Services.interfaces;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using StoredProcedureEFCore;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace BRB.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IContactInfoService _contactInfoService;

        public ResumeController(IResumeService resumeService,IContactInfoService contactInfoService)
        {
            _resumeService = resumeService;
            _contactInfoService = contactInfoService;
        }

        public IActionResult Home()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult CreateResumeProfile(ResumeViewModels resumeViewModels)
        {
            int resumeId = 0;
            int userId = Convert.ToInt16(HttpContext.Session.GetString("_userId"));
            //resumeViewModels.ResumeId = resumeId;
            var isExist = _resumeService.IsUserExist(userId);
            if (!isExist)
            {
            var resumeRecord = _resumeService.CreateResumeMaster(resumeViewModels);
            HttpContext.Session.SetString("_resumeId", resumeRecord.ResumeId.ToString());
            }
            else
            {
                resumeId = _resumeService.UpdateResumeMaster(resumeViewModels);
                HttpContext.Session.SetString("_resumeId", resumeId.ToString());
            }
            return Json("");
        }


        public IActionResult GenerateResume()
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            ResumeGenerateModel model = new ResumeGenerateModel();
            var ds = SqlHelper.GetDataSet("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetResume", CommandType.StoredProcedure, new SqlParameter("ResumeId", sessionData.ResumeId));

            if (ds.Tables.Count >= 15)
            {
                model.Education = ds.Tables[0].ToList_<Education>().FirstOrDefault();
                model.Contact = ds.Tables[1].ToList_<ContactInfo>().FirstOrDefault();
                model.ObjectiveSummary = ds.Tables[2].ToList_<ObjectiveSummary>().FirstOrDefault();
                model.Colleges = ds.Tables[3].ToList_<College>();
                model.AcademicScholarships = ds.Tables[4].ToList_<AcademicScholarship>();
                model.AcademicHonors = ds.Tables[5].ToList_<AcademicHonor>();
                model.OverseasExperience = ds.Tables[6].ToList_<OverseasExperience>().FirstOrDefault();
                model.OverseasStudies = ds.Tables[7].ToList_<OverseasStudy>();
                model.MilitaryExperiences = ds.Tables[8].ToList_<MilitaryExperience>().FirstOrDefault();
                model.MilitaryPositions = ds.Tables[9].ToList_<MilitaryPosition>();
                model.OrgExperience = ds.Tables[10].ToList_<OrgExperience>().FirstOrDefault();
                model.Organizations = ds.Tables[11].ToList_<Organization>();
                model.VolunteerExperience = ds.Tables[12].ToList_<VolunteerExperience>().FirstOrDefault();
                model.VolunteerOrgs = ds.Tables[13].ToList_<VolunteerOrg>();
                model.VolunteerPositions = ds.Tables[14].ToList_<VolunteerPosition>();
                model.Professional = ds.Tables[15].ToList_<Professional>().FirstOrDefault();
                model.Licenses = ds.Tables[16].ToList_<License>();
                model.Certificates = ds.Tables[17].ToList_<Certificate>();
                model.Affiliations = ds.Tables[18].ToList_<Affiliation>();
                model.AffiliationPositions = ds.Tables[19].ToList_<AffiliationPosition>();
                model.TechnicalSkill = ds.Tables[20].ToList_<TechnicalSkill>().FirstOrDefault();
                model.LanguageSkill = ds.Tables[21].ToList_<LanguageSkill>().FirstOrDefault();
                model.Languages = ds.Tables[22].ToList_<Language>();
                model.UserProfile = ds.Tables[23].ToList_<UserProfile>().FirstOrDefault();
            }

            return View("ResumePdf",model);
        }
        public IActionResult ContactInfo()
        {
            
            return View();
        }

      

        public IActionResult Objective()
        {
            return View();
        }

        public IActionResult Education()
        {
            return View();
        }

        public IActionResult OverseasStudy()
        {
            return View();
        }
        public IActionResult WorkExperience()
        {
            return View();
        }

        public IActionResult Military()
        {
            return View();
        }
        public IActionResult Organizations()
        {
            return View();
        }
        public IActionResult CommunityService()
        {
            return View();
        }
        public IActionResult ComputerAndTechnicalSkills()
        {
            return View();
        }
        public IActionResult Certifications()
        {
            return View();
        }

        public IActionResult LanguagesSKills()
        {
            return View();
        }

      

    }
}
