﻿using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
//using BusinessObjects.Models.MetaData.New;
using BusinessObjects.Services.interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using StoredProcedureEFCore;
using System.Data;
using static BusinessObjects.Helper.ApplicationHelper;

namespace BRB.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IContactInfoService _contactInfoService;
        IWebHostEnvironment _webHostEnvironment;
        public ResumeController(IResumeService resumeService, IContactInfoService contactInfoService, IWebHostEnvironment webHostEnvironment)
        {
            _resumeService = resumeService;
            _contactInfoService = contactInfoService;
            _webHostEnvironment = webHostEnvironment;
        }
        //[OutputCache(NoStore = true, Duration = 0, PolicyName = "OutputCacheWithAuthPolicy")]
        public IActionResult Home()
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            ViewBag.GeneratedFileName = record.GeneratedFileName;
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

            return View("ResumePdf", model);
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


        public async Task<IActionResult> GenerateResumeOnWord(string font)
        {

            SendResume(_webHostEnvironment.WebRootPath + "/downloads/resume - 77a529ed-d907-4268-a8f5-6e11e87e5e53.docx", "bilalkhan.19@outlook.com");
            System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/log1.txt", "request received");
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Message = "";
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            ResumeGenerateModel model = new ResumeGenerateModel();
            var ds = SqlHelper.GetDataSet("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetResume", CommandType.StoredProcedure, new SqlParameter("ResumeId", sessionData.ResumeId));

            if (ds.Tables.Count >= 15)
            {

                System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/log2.txt", "ds.Tables.Count >= 15");
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

            System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/log3.txt", "tring htmlValue = await this.RenderViewAsync(\"Resumepdf\", model);");
            string htmlValue = await this.RenderViewAsync("Resumepdf", model);

            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(
                       generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document(new Body()).Save(mainPart);
                    }

                    HtmlConverter converter = new HtmlConverter(mainPart);
                    Body body = mainPart.Document.Body;

                    var paragraphs = converter.Parse(htmlValue);
                    for (int i = 0; i < paragraphs.Count; i++)
                    {
                        body.Append(paragraphs[i]);
                    }

                    System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/log4.txt", "ring filename = \"resume - \" + Guid.NewGuid() + \".docx;");
                    mainPart.Document.Save();



                }
                System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/log5.txt", "ring filename = \"resume - \" + Guid.NewGuid() + \".docx;");
                string filename = "resume - " + Guid.NewGuid() + ".docx";
                System.IO.File.WriteAllBytes(_webHostEnvironment.WebRootPath + "/downloads/" + filename, generatedDocument.ToArray());
                var record = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                record.GeneratedFileName = filename;
                record.ChosenFont = font;
                record.LastModDate = DateTime.Today;
                record.GeneratedDate = DateTime.Today;
                _dbContext.SaveChanges();
                ajaxResponse.Message = "email has been sent to you email address bilalkhan.19@outlook.com";
                ajaxResponse.Data = filename;
                ajaxResponse.Success = true;
                SendResume(_webHostEnvironment.WebRootPath + "/downloads/" + filename, "bilalkhan.19@outlook.com");

                //return File(
                //fileContents: generatedDocument.ToArray(),
                //contentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                //fileDownloadName: filename);
                //TempData["msg"] = "email has been sent to you email address bilalkhan.19@outlook.com";
                return Json(ajaxResponse);
            }


        }



    }
}
