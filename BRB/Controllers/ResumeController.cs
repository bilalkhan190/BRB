using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services;
//using BusinessObjects.Models.MetaData.New;
using BusinessObjects.Services.interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoredProcedureEFCore;
using System.Data;
using System.Net.Mail;
using System.Text;
using static BusinessObjects.Helper.ApplicationHelper;

namespace BRB.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IContactInfoService _contactInfoService;
        private readonly IUserProfileService _userProfileService;
        IWebHostEnvironment _webHostEnvironment;
        public ResumeController(IResumeService resumeService, IContactInfoService contactInfoService, IWebHostEnvironment webHostEnvironment, IUserProfileService userProfileService)
        {
            _resumeService = resumeService;
            _contactInfoService = contactInfoService;
            _webHostEnvironment = webHostEnvironment;
            _userProfileService = userProfileService;
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
            CreateWorkExperience();
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            var companies = _dbContext.WorkCompanies.Where(x => x.WorkExperienceId == record.WorkExperienceId).ToList();
            companies.ForEach(f =>
            {
                var positions = _dbContext.WorkPositions.Where(x => x.CompanyId == f.CompanyId).ToList();
                positions.ForEach(a =>
                {
                    var awards = _dbContext.JobAwards.Where(x => x.CompanyJobId == a.PositionId).ToList();
                    a.JobAwards = awards;
                    a.workRespQuestions = _dbContext.WorkRespQuestions.Where(x => x.PositionId == a.PositionId).ToList();
                    a.responsibilityOptions = _dbContext.ResponsibilityOptionsResponses.Where(x => x.PositionId == a.PositionId).ToList();

                });
                f.Positions = positions;
            });
            record.Companies = companies;


            return View(record);
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

        public IActionResult CreateWorkExperience()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (record == null)
            {
                WorkExperience workExperience = new WorkExperience()
                {
                    ResumeId = sessionData.ResumeId,
                    CreatedDate = DateTime.Today,
                    IsComplete = false,

                };
                _dbContext.WorkExperiences.Add(workExperience);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }

        public async Task<IActionResult> GenerateResumeOnWord(string font)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Message = "";
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "Hit approved");
            ResumeGenerateModel model = new ResumeGenerateModel();
            try
            {
                var ds = SqlHelper.GetDataSet("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetResume", CommandType.StoredProcedure, new SqlParameter("ResumeId", sessionData.ResumeId));
                ViewBag.fonts = font;
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
                    model.Licenses = ds.Tables[16].ToList_<BusinessObjects.Models.License>();
                    model.Certificates = ds.Tables[17].ToList_<Certificate>();
                    model.Affiliations = ds.Tables[18].ToList_<Affiliation>();
                    model.AffiliationPositions = ds.Tables[19].ToList_<AffiliationPosition>();
                    model.TechnicalSkill = ds.Tables[20].ToList_<TechnicalSkill>().FirstOrDefault();
                    model.LanguageSkill = ds.Tables[21].ToList_<LanguageSkill>().FirstOrDefault();
                    model.Languages = ds.Tables[22].ToList_<Language>();
                    model.UserProfile = ds.Tables[23].ToList_<UserProfile>().FirstOrDefault();
                    model.WorkExperience = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    model.WorkExperience.Companies = _dbContext.WorkCompanies.Where(x => x.WorkExperienceId == model.WorkExperience.WorkExperienceId).ToList();
                    foreach (var company in model.WorkExperience.Companies)
                    {
                        company.Positions = _dbContext.WorkPositions.Where(x => x.CompanyId == company.CompanyId).ToList();

                        foreach (var job in company.Positions)
                        {
                            job.responsibilityOptions = _dbContext.ResponsibilityOptionsResponses.Where(x => x.PositionId == job.PositionId).ToList();
                            job.responsibilityOptions.ForEach(record =>
                            {
                                record.Caption = _dbContext.ResponsibilityOptions.FirstOrDefault(x => x.RespOptionId == record.ResponsibilityOption)?.Caption;
                            });
                            job.workRespQuestions = (from question in _dbContext.ResponsibilityQuestions
                                                     join workResQuestion in _dbContext.WorkRespQuestions on question.RespQuestionId equals workResQuestion.RespQuestionId
                                                     where workResQuestion.PositionId == job.PositionId
                                                     select new WorkRespQuestion
                                                     {
                                                         Answer = workResQuestion.Answer,
                                                         Question = question.Caption
                                                     }
                                                     ).ToList();
                            job.JobAwards = _dbContext.JobAwards.Where(x => x.CompanyJobId == job.PositionId).ToList();
                        }
                    }
                }
                System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "Data fetched");
                string htmlValue = await this.RenderViewAsync("Resumepdf", model);
                System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "Html generated");
                using (MemoryStream generatedDocument = new MemoryStream())
                {
                    using (WordprocessingDocument package = WordprocessingDocument.Create(
                           generatedDocument, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = package.MainDocumentPart;
                        if (mainPart == null)
                        {
                            mainPart = package.AddMainDocumentPart();
                            new Document(new DocumentFormat.OpenXml.Wordprocessing.Body()).Save(mainPart);
                        }

                        HtmlConverter converter = new HtmlConverter(mainPart);
                        DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.Body;

                        var paragraphs = converter.Parse(htmlValue);
                        for (int i = 0; i < paragraphs.Count; i++)
                        {

                            body.Append(paragraphs[i]);
                        }

                        mainPart.Document.Save();



                    }
                    System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "word generated");
                    string filename = "resume - " + Guid.NewGuid() + ".docx";
                    System.IO.File.WriteAllBytes(_webHostEnvironment.WebRootPath + "/downloads/" + filename, generatedDocument.ToArray());
                    var record = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    record.GeneratedFileName = filename;
                    record.ChosenFont = font;
                    record.LastModDate = DateTime.Today;
                    record.GeneratedDate = DateTime.Today;
                    _dbContext.SaveChanges();
                    System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "attachement saved email started");
                    //SendResume(_webHostEnvironment.WebRootPath + "/downloads/" + filename, "bilalkhan.19@outlook.com");
                    SendEmailAttachment(sessionData.UserName, new Attachment(_webHostEnvironment.WebRootPath + "/downloads/" + filename));
                    System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "email sent");
                    //SendEmail(sessionData.UserName,)
                    ajaxResponse.Message = $"Email has been sent to your email address {sessionData.UserName} ";
                    ajaxResponse.Data = filename;
                    ajaxResponse.Success = true;
                  
                }
            }
            catch (Exception ex)
            {
                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;
                
            }
            return Json(ajaxResponse);

            //Aspose

            //Aspose.Words.Document doc = new Aspose.Words.Document();
            //DocumentBuilder builder = new DocumentBuilder(doc);
            //builder.InsertHtml(htmlValue);
            //MemoryStream stream = new MemoryStream();
            //doc.Save(stream, SaveFormat.Docx);
            //return File(
            //    fileContents: stream.ToArray(),
            //    contentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            //    fileDownloadName: "output.docx");

        }


        public IActionResult VoucherVerification()
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            if (!string.IsNullOrEmpty(sessionData.VoucherCode))
            {
                return RedirectToAction("home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult VerifyVoucher(string voucherCode)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Unable to verify code";
            ajaxResponse.Redirect = "";

            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var voucherExists = _dbContext.Vouchers.FirstOrDefault(x => x.Code == voucherCode);

            if (voucherExists != null)
            {
                if (voucherExists.EffectiveDate.Date <= DateTime.Now.Date &&  DateTime.Now.Date <= voucherExists.ExpirationDate.Date)
                {
                    var voucherRecords = _dbContext.Resumes.Where(x => x.VoucherCode == voucherCode).ToList();
                    if (voucherRecords.Count() >= voucherExists.InitialCount)
                    {
                        ajaxResponse.Message = "Voucher code has already been claimed. Please correct the code and try again.";
                    }
                    else
                    {
                        Resume resume = _dbContext.Resumes.FirstOrDefault(x => x.UserId == sessionData.UserId);
                        resume.VoucherCode = voucherCode;
                        _dbContext.Resumes.Update(resume);

                        UserProfile user = _dbContext.UserProfiles.FirstOrDefault(x => x.UserId == sessionData.UserId);
                        user.IsActive = true;
                        _dbContext.UserProfiles.Update(user);

                        _dbContext.SaveChanges();
                        sessionData.VoucherCode = voucherCode;
                        HttpContext.Session.SetString("_userData", JsonConvert.SerializeObject(sessionData));
                        ajaxResponse.Success = true;
                        ajaxResponse.Message = "Code verified redirecting...";
                        ajaxResponse.Redirect = "/Resume/Home";
                    }
                }
                else
                {
                    ajaxResponse.Message = "Voucher doesn't exist or has been expired";
                }                                               
               
            }           
            return Json(ajaxResponse);
        }



        public IActionResult GenerateWordDocument(string font)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Message = "";
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "Hit approved");
            ResumeGenerateModel model = new ResumeGenerateModel();
            try
            {
                var ds = SqlHelper.GetDataSet("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetResume", CommandType.StoredProcedure, new SqlParameter("ResumeId", sessionData.ResumeId));
                ViewBag.fonts = font;
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
                    model.Licenses = ds.Tables[16].ToList_<BusinessObjects.Models.License>();
                    model.Certificates = ds.Tables[17].ToList_<Certificate>();
                    model.Affiliations = ds.Tables[18].ToList_<Affiliation>();
                    model.AffiliationPositions = ds.Tables[19].ToList_<AffiliationPosition>();
                    model.Affiliations.ForEach(x =>
                    {
                        x.AffiliationPositions = model.AffiliationPositions.Where(m => m.AffiliationId == x.AffiliationId).ToList();
                    });
                    model.TechnicalSkill = ds.Tables[20].ToList_<TechnicalSkill>().FirstOrDefault();
                    model.LanguageSkill = ds.Tables[21].ToList_<LanguageSkill>().FirstOrDefault();
                    model.Languages = ds.Tables[22].ToList_<Language>();
                    model.UserProfile = ds.Tables[23].ToList_<UserProfile>().FirstOrDefault();
                    model.OrgPositions = ds.Tables[24].ToList_<OrgPosition>().ToList();                    
                    model.WorkExperience = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    model.WorkExperience.Companies = _dbContext.WorkCompanies.Where(x => x.WorkExperienceId == model.WorkExperience.WorkExperienceId).ToList();
                    foreach (var company in model.WorkExperience.Companies)
                    {
                        company.Positions = _dbContext.WorkPositions.Where(x => x.CompanyId == company.CompanyId).ToList();

                        foreach (var job in company.Positions)
                        {
                            job.JobResponsibilityText = _dbContext.JobCategoryLists.FirstOrDefault(x => x.JobCategoryId == job.JobResponsibilityId)?.JobCategoryDesc;
                            job.responsibilityOptions = _dbContext.ResponsibilityOptionsResponses.Where(x => x.PositionId == job.PositionId).ToList();
                            job.responsibilityOptions.ForEach(record =>
                            {
                                record.Caption = _dbContext.ResponsibilityOptions.FirstOrDefault(x => x.RespOptionId == record.ResponsibilityOption)?.Caption;
                            });
                            job.workRespQuestions = (from question in _dbContext.ResponsibilityQuestions
                                                     join workResQuestion in _dbContext.WorkRespQuestions on question.RespQuestionId equals workResQuestion.RespQuestionId
                                                     where workResQuestion.PositionId == job.PositionId
                                                     select new WorkRespQuestion
                                                     {
                                                         Answer = workResQuestion.Answer,
                                                         Question = question.Caption
                                                     }
                                                     ).ToList();
                            job.JobAwards = _dbContext.JobAwards.Where(x => x.CompanyJobId == job.PositionId).ToList();
                        }
                    }
                }


                string resume = CreateResume(model, font);

                string filename = "resume" + Guid.NewGuid();
                string str = filename;
                int num = 1;
                System.IO.File.Create(_webHostEnvironment.WebRootPath + "/downloads/" + str + ".doc");
                while (System.IO.File.Exists(_webHostEnvironment.WebRootPath + "/downloads/" + str + ".doc"))
                    str = string.Format("{0}_{1}", (object)filename, (object)num++);
                string fullFilename = _webHostEnvironment.WebRootPath + "/downloads/" + str + ".doc";                
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(fullFilename, false))
                    {
                        streamWriter.Write(resume);
                        streamWriter.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                ViewBag.GeneratedFileName = str + ".doc";
                var record = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                record.GeneratedFileName = str + ".doc";
                record.ChosenFont = font;
                record.LastModDate = DateTime.Today;
                record.GeneratedDate = DateTime.Today;
                _dbContext.SaveChanges();
                SendEmailAttachment(sessionData.UserName, new Attachment(_webHostEnvironment.WebRootPath + "/downloads/" + str + ".doc"));
                ajaxResponse.Message = $"Email has been sent to your email address {sessionData.UserName} ";
                ajaxResponse.Data = str + ".doc";
                ajaxResponse.Success = true;

                
            }


            catch (Exception ex)
            {
                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;

            }
            return Json(ajaxResponse);
        }

        private static string CreateResume(ResumeGenerateModel resume, string font)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version='1.0'?><?mso-application progid='Word.Document'?><w:wordDocument xmlns:w='http://schemas.microsoft.com/office/word/2003/wordml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w10='urn:schemas-microsoft-com:office:word' xmlns:sl='http://schemas.microsoft.com/schemaLibrary/2003/core' xmlns:aml='http://schemas.microsoft.com/aml/2001/core' xmlns:wx='http://schemas.microsoft.com/office/word/2003/auxHint' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882' xmlns:st1='urn:schemas-microsoft-com:office:smarttags' w:macrosPresent='no' w:embeddedObjPresent='no' w:ocxPresent='no' xml:space='preserve'><o:SmartTagType o:namespaceuri='urn:schemas-microsoft-com:office:smarttags' o:name='Street'/><w:fonts><w:defaultFonts w:ascii='[Font]' w:fareast='[Font]' w:h-ansi='[Font]' w:cs='[Font]'/></w:fonts><w:lists><w:listDef w:listDefId='0'><w:lsid w:val='0588744C'/><w:plt w:val='Multilevel'/><w:tmpl w:val='0409001F'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:lvlText w:val='%1.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='360'/></w:tabs><w:ind w:left='360' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:lvlText w:val='%1.%2.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='792'/></w:tabs><w:ind w:left='792' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1224'/></w:tabs><w:ind w:left='1224' w:hanging='504'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1800'/></w:tabs><w:ind w:left='1728' w:hanging='648'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2520'/></w:tabs><w:ind w:left='2232' w:hanging='792'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2880'/></w:tabs><w:ind w:left='2736' w:hanging='936'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3600'/></w:tabs><w:ind w:left='3240' w:hanging='1080'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3960'/></w:tabs><w:ind w:left='3744' w:hanging='1224'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.%9.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='4680'/></w:tabs><w:ind w:left='4320' w:hanging='1440'/></w:pPr></w:lvl></w:listDef><w:listDef w:listDefId='1'><w:lsid w:val='3B02017F'/><w:plt w:val='Multilevel'/><w:tmpl w:val='0409001D'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:lvlText w:val='%1)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='360'/></w:tabs><w:ind w:left='360' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:nfc w:val='4'/><w:lvlText w:val='%2)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='720'/></w:tabs><w:ind w:left='720' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:nfc w:val='2'/><w:lvlText w:val='%3)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1080'/></w:tabs><w:ind w:left='1080' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:lvlText w:val='(%4)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1440'/></w:tabs><w:ind w:left='1440' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:nfc w:val='4'/><w:lvlText w:val='(%5)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1800'/></w:tabs><w:ind w:left='1800' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:nfc w:val='2'/><w:lvlText w:val='(%6)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2160'/></w:tabs><w:ind w:left='2160' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:lvlText w:val='%7.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2520'/></w:tabs><w:ind w:left='2520' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:nfc w:val='4'/><w:lvlText w:val='%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2880'/></w:tabs><w:ind w:left='2880' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:nfc w:val='2'/><w:lvlText w:val='%9.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3240'/></w:tabs><w:ind w:left='3240' w:hanging='360'/></w:pPr></w:lvl></w:listDef><w:listDef w:listDefId='2'><w:lsid w:val='447F7E44'/><w:plt w:val='Multilevel'/><w:tmpl w:val='82AA5C2C'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:nfc w:val='23'/><w:lvlText w:val='?'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='360'/></w:tabs><w:ind w:left='360' w:hanging='360'/></w:pPr><w:rPr><w:rFonts w:ascii='Symbol' w:h-ansi='Symbol' w:hint='default'/></w:rPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:lvlText w:val='%1.%2.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='792'/></w:tabs><w:ind w:left='792' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1224'/></w:tabs><w:ind w:left='1224' w:hanging='504'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1800'/></w:tabs><w:ind w:left='1728' w:hanging='648'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2520'/></w:tabs><w:ind w:left='2232' w:hanging='792'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2880'/></w:tabs><w:ind w:left='2736' w:hanging='936'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3600'/></w:tabs><w:ind w:left='3240' w:hanging='1080'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3960'/></w:tabs><w:ind w:left='3744' w:hanging='1224'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.%9.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='4680'/></w:tabs><w:ind w:left='4320' w:hanging='1440'/></w:pPr></w:lvl></w:listDef><w:listDef w:listDefId='3'><w:lsid w:val='458C501E'/><w:plt w:val='Multilevel'/><w:tmpl w:val='00000000'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:lvlText w:val='%1.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='360'/></w:tabs><w:ind w:left='360' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:lvlText w:val='%1.%2.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='792'/></w:tabs><w:ind w:left='792' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1224'/></w:tabs><w:ind w:left='1224' w:hanging='504'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1800'/></w:tabs><w:ind w:left='1728' w:hanging='648'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2520'/></w:tabs><w:ind w:left='2232' w:hanging='792'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2880'/></w:tabs><w:ind w:left='2736' w:hanging='936'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3600'/></w:tabs><w:ind w:left='3240' w:hanging='1080'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3960'/></w:tabs><w:ind w:left='3744' w:hanging='1224'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.%9.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='4680'/></w:tabs><w:ind w:left='4320' w:hanging='1440'/></w:pPr></w:lvl></w:listDef><w:listDef w:listDefId='4'><w:lsid w:val='473E6307'/><w:plt w:val='Multilevel'/><w:tmpl w:val='00000000'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:lvlText w:val='%1.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='360'/></w:tabs><w:ind w:left='360' w:hanging='360'/></w:pPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:lvlText w:val='%1.%2.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='792'/></w:tabs><w:ind w:left='792' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1224'/></w:tabs><w:ind w:left='1224' w:hanging='504'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1800'/></w:tabs><w:ind w:left='1728' w:hanging='648'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2520'/></w:tabs><w:ind w:left='2232' w:hanging='792'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2880'/></w:tabs><w:ind w:left='2736' w:hanging='936'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3600'/></w:tabs><w:ind w:left='3240' w:hanging='1080'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3960'/></w:tabs><w:ind w:left='3744' w:hanging='1224'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.%9.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='4680'/></w:tabs><w:ind w:left='4320' w:hanging='1440'/></w:pPr></w:lvl></w:listDef><w:listDef w:listDefId='5'><w:lsid w:val='59473144'/><w:plt w:val='Multilevel'/><w:tmpl w:val='8104030E'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:nfc w:val='23'/><w:lvlText w:val='·'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='360'/></w:tabs><w:ind w:left='360' w:hanging='360'/></w:pPr><w:rPr><w:rFonts w:ascii='Symbol' w:h-ansi='Symbol' w:hint='default'/></w:rPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:lvlText w:val='%1.%2.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='792'/></w:tabs><w:ind w:left='792' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1224'/></w:tabs><w:ind w:left='1224' w:hanging='504'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1800'/></w:tabs><w:ind w:left='1728' w:hanging='648'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2520'/></w:tabs><w:ind w:left='2232' w:hanging='792'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='2880'/></w:tabs><w:ind w:left='2736' w:hanging='936'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3600'/></w:tabs><w:ind w:left='3240' w:hanging='1080'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='3960'/></w:tabs><w:ind w:left='3744' w:hanging='1224'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:lvlText w:val='%1.%2.%3.%4.%5.%6.%7.%8.%9.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='4680'/></w:tabs><w:ind w:left='4320' w:hanging='1440'/></w:pPr></w:lvl></w:listDef><w:listDef w:listDefId='6'><w:lsid w:val='733C1BB9'/><w:plt w:val='Multilevel'/><w:tmpl w:val='04090023'/><w:lvl w:ilvl='0'><w:start w:val='1'/><w:nfc w:val='1'/><w:lvlText w:val='Article %1.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1440'/></w:tabs><w:ind w:left='0' w:first-line='0'/></w:pPr></w:lvl><w:lvl w:ilvl='1'><w:start w:val='1'/><w:nfc w:val='22'/><w:isLgl/><w:lvlText w:val='Section %1.%2'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1080'/></w:tabs><w:ind w:left='0' w:first-line='0'/></w:pPr></w:lvl><w:lvl w:ilvl='2'><w:start w:val='1'/><w:nfc w:val='4'/><w:lvlText w:val='(%3)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='720'/></w:tabs><w:ind w:left='720' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='3'><w:start w:val='1'/><w:nfc w:val='2'/><w:lvlText w:val='(%4)'/><w:lvlJc w:val='right'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='864'/></w:tabs><w:ind w:left='864' w:hanging='144'/></w:pPr></w:lvl><w:lvl w:ilvl='4'><w:start w:val='1'/><w:lvlText w:val='%5)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1008'/></w:tabs><w:ind w:left='1008' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='5'><w:start w:val='1'/><w:nfc w:val='4'/><w:lvlText w:val='%6)'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1152'/></w:tabs><w:ind w:left='1152' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='6'><w:start w:val='1'/><w:nfc w:val='2'/><w:lvlText w:val='%7)'/><w:lvlJc w:val='right'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1296'/></w:tabs><w:ind w:left='1296' w:hanging='288'/></w:pPr></w:lvl><w:lvl w:ilvl='7'><w:start w:val='1'/><w:nfc w:val='4'/><w:lvlText w:val='%8.'/><w:lvlJc w:val='left'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1440'/></w:tabs><w:ind w:left='1440' w:hanging='432'/></w:pPr></w:lvl><w:lvl w:ilvl='8'><w:start w:val='1'/><w:nfc w:val='2'/><w:lvlText w:val='%9.'/><w:lvlJc w:val='right'/><w:pPr><w:tabs><w:tab w:val='list' w:pos='1584'/></w:tabs><w:ind w:left='1584' w:hanging='144'/></w:pPr></w:lvl></w:listDef><w:list w:ilfo='1'><w:ilst w:val='4'/></w:list><w:list w:ilfo='2'><w:ilst w:val='0'/></w:list><w:list w:ilfo='3'><w:ilst w:val='1'/></w:list><w:list w:ilfo='4'><w:ilst w:val='6'/></w:list><w:list w:ilfo='5'><w:ilst w:val='5'/></w:list><w:list w:ilfo='6'><w:ilst w:val='3'/></w:list><w:list w:ilfo='7'><w:ilst w:val='2'/></w:list></w:lists><w:docPr><w:view w:val='print'/><w:zoom w:percent='100'/></w:docPr><w:body><w:sectPr><w:pgMar w:top='720' w:right='720' w:bottom='720' w:left='720' />".Replace("[Font]", font));
            stringBuilder.Append(GetContactInfoContent(resume.Contact));
            stringBuilder.Append(GetSummaryContent(resume.ObjectiveSummary));
            stringBuilder.Append(GetEducationContent(resume.Colleges, resume.AcademicHonors, resume.AcademicScholarships));
            stringBuilder.Append(GetOverseasContent(resume.OverseasExperience, resume.OverseasStudies));
            stringBuilder.Append(GetWorkExperienceContent(resume.WorkExperience));
            stringBuilder.Append(GetMilitaryExperienceContent(resume.MilitaryExperiences, resume.MilitaryPositions));
            stringBuilder.Append(GetOrgAndVolunteerExperienceContent(resume.OrgExperience, resume.Organizations, resume.OrgPositions, resume.VolunteerExperience, resume.VolunteerOrgs, resume.VolunteerPositions));
            stringBuilder.Append(GetProfessionalContent(resume));
            stringBuilder.Append(GetTechnicalContent(resume));
            stringBuilder.Append(GetLanguageContent(resume));
            stringBuilder.Append("</w:sectPr></w:body></w:wordDocument>");
            stringBuilder.Replace("&", "&amp;");
            return stringBuilder.ToString();
        }




        public static string GetContactInfoContent(ContactInfo resume)
        {
           
            StringBuilder stringBuilder = new StringBuilder();
            string str1 = "<w:p><w:pPr><w:jc w:val='center' /></w:pPr><w:r><w:rPr><w:b /><w:sz w:val='32'/><w:sz-cs w:val='32'/></w:rPr><w:t>[FirstName] [LastName]</w:t></w:r></w:p>".Replace("[FirstName]", resume.FirstName).Replace("[LastName]", resume.LastName);
            stringBuilder.Append(str1);
            string str2 = "<w:p><w:pPr><w:jc w:val='center' /></w:pPr><w:r><w:rPr><w:sz w:val='20'/></w:rPr><w:t>[Address1]</w:t></w:r></w:p>".Replace("[Address1]", resume.Address1);
            stringBuilder.Append(str2);
            if (!string.IsNullOrEmpty(resume.Address2))
            {
                string str3 = "<w:p><w:pPr><w:jc w:val='center' /></w:pPr><w:r><w:rPr><w:sz w:val='20'/></w:rPr><w:t>[Address2]</w:t></w:r></w:p>".Replace("[Address2]", resume.Address2);
                stringBuilder.Append(str3);
            }
            string str4 = "<w:p><w:pPr><w:jc w:val='center' /></w:pPr><w:r><w:rPr><w:sz w:val='20'/></w:rPr><w:t>[City], [State] [Zip]</w:t></w:r></w:p>".Replace("[City]", resume.City).Replace("[State]", resume.StateAbbr).Replace("[Zip]", resume.ZipCode);
            stringBuilder.Append(str4);
            if (!string.IsNullOrEmpty(resume.Phone))
            {
                string str5 = "<w:p><w:pPr><w:jc w:val='center' /></w:pPr><w:r><w:rPr><w:sz w:val='20'/></w:rPr><w:t>[PhoneNumber]</w:t></w:r></w:p>".Replace("[PhoneNumber]", Convert.ToDouble(((string)resume.Phone).Replace("-", "")).ToString("(###)###-####"));
                stringBuilder.Append(str5);
            }
            string str6 = "<w:p><w:pPr><w:jc w:val='center' /></w:pPr><w:r><w:rPr><w:sz w:val='20'/></w:rPr><w:t>[Email]</w:t></w:r></w:p>".Replace("[Email]", resume.Email);
            stringBuilder.Append(str6);
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='16'/><w:sz-cs w:val='16'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        public static string GetSummaryContent(ObjectiveSummary resume)
        {
            string str1 = $"<w:p><w:pPr> <w:jc w:val='center'/><w:ind w:left='1260' w:hanging='1260'/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>{resume.ObjectiveType}:</w:t></w:r><w:r><w:rPr><w:b/></w:rPr><w:tab wx:wTab='225' wx:tlc='none' wx:cTlc='3'/></w:r><w:r><w:rPr><w:sz w:val='22'/></w:rPr><w:t>To utilize my {resume.ObjectiveDesc1}, {resume.ObjectiveDesc2} and {resume.ObjectiveDesc3} skills to secure a {resume.PositionTypeDesc} in a {resume.CurrentCompanyType} to increase revenue.</w:t></w:r></w:p>";
            return str1;
        }

        public static string GetEducationContent(List<College> colleges, List<AcademicHonor> Honors, List<AcademicScholarship> scholarships)
        {
            //JToken jtoken = JObject.Parse(resume.Data)["education"];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<w:p><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Education:</w:t></w:r></w:p>");
            stringBuilder.Append(GetCollegeContent(colleges, Honors, scholarships));
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        private static string GetCollegeContent(List<College> colleges, List<AcademicHonor> Honors, List<AcademicScholarship> scholarships)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var college in colleges)
            {
                if (num > 1 && num <= colleges.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='10'/><w:sz-cs w:val='10'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[CollegeName]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[GradDate]</w:t></w:r></w:p>".Replace("[CollegeName]", college.CollegeName).Replace("[City]", college.CollegeCity).Replace("[State]", college.CollegeStateAbbr).Replace("[GradDate]",college.GradDate == null ? "" : Convert.ToDateTime(college.GradDate).ToString("MMMM") + " " + Convert.ToDateTime(college.GradDate).ToString("yyyy"));
                stringBuilder.Append(str1);
                if (!string.IsNullOrEmpty(college.HonorProgram))
                    stringBuilder.Append("<w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Program]</w:t></w:r></w:p>".Replace("[Program]", college.HonorProgram));
                if (college.DegreeDesc.ToLower().Equals("general") && college.MajorDesc.ToLower().Equals("general"))
                {
                    if (college.IncludeGpa == true)
                        stringBuilder.Append("<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>GPA:  [GPA]</w:t></w:r></w:p>".Replace("[GPA]", college.Gpa));
                }
                else
                {
                    string str2 = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Degree] - [Major]</w:t></w:r>";
                    string str3 = !college.DegreeDesc.ToLower().Equals("other") ? str2.Replace("[Degree]", college.DegreeDesc) : str2.Replace("[Degree]", college.DegreeOther);
                    string str4 = !college.MajorDesc.ToLower().Equals("other") ? str3.Replace("[Major]", college.MajorDesc) : str3.Replace("[Major]", college.MajorOther);
                    stringBuilder.Append(str4);
                    if (college.IncludeGpa == true)
                        stringBuilder.Append("<w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>GPA:  [GPA]</w:t></w:r></w:p>".Replace("[GPA]",college.Gpa));
                    else
                        stringBuilder.Append("<w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t></w:t></w:r></w:p>");
                }
                if (!string.IsNullOrEmpty(college.MajorSpecialtyDesc))
                {
                    if (college.MajorSpecialtyDesc.ToLower().Equals("other"))
                        stringBuilder.Append("<w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Specialty in [Specialty]</w:t></w:r></w:p>".Replace("[Specialty]", college.MajorSpecialtyOther));
                    else
                        stringBuilder.Append("<w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Specialty in [Specialty]</w:t></w:r></w:p>".Replace("[Specialty]", college.MajorSpecialtyDesc));
                }
                if (!string.IsNullOrEmpty(college.MinorDesc))
                {
                    if (college.MinorDesc.ToLower().Equals("other"))
                        stringBuilder.Append("<w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Minor:  [Minor]</w:t></w:r></w:p>".Replace("[Minor]", college.MinorOther));
                    else
                        stringBuilder.Append("<w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Minor:  [Minor]</w:t></w:r></w:p>".Replace("[Minor]", college.MinorDesc));
                }
                if (!string.IsNullOrEmpty(college.CertificateDesc))
                {
                    if (college.CertificateDesc.ToLower().Equals("other"))
                        stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p><w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Certificate in [Certificate]</w:t></w:r></w:p>".Replace("[Certificate]", college.CertificateOther));
                    else
                        stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p><w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Certificate in [Certificate]</w:t></w:r></w:p>".Replace("[Certificate]", college.CertificateDesc));
                }
                if (!string.IsNullOrEmpty(college.HonorProgram))
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p><w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[HonorProgram]</w:t></w:r></w:p>".Replace("[HonorProgram]", college.HonorProgram));
                string academicHonorContent = GetAcademicHonorContent(Honors);
                if (!string.IsNullOrEmpty(academicHonorContent))
                    stringBuilder.Append(academicHonorContent);
                string scholarshipContent = GetScholarshipContent(scholarships);
                if (!string.IsNullOrEmpty(scholarshipContent))
                    stringBuilder.Append(scholarshipContent);
                ++num;
            }
            return stringBuilder.ToString();
        }


        private static string GetAcademicHonorContent(List<AcademicHonor> honors)
        {
            if (honors == null || honors.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            stringBuilder.Append("<w:p><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>");
            int num = 1;
            foreach (var honor in honors)
            {
                if (num > 1)
                    stringBuilder.Append(", ");
                string str = "[Name] [Month] [Year]".Replace("[Name]", honor.HonorName).Replace("[Month]", honor.HonorMonth).Replace("[Year]", honor.HonorYear);
                stringBuilder.Append(str);
                ++num;
            }
            stringBuilder.Append("</w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        private static string GetScholarshipContent(List<AcademicScholarship> scholars)
        {
            if (scholars == null || scholars.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var scholar in scholars)
            {
                stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Name]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[Month] [Year]</w:t></w:r></w:p>".Replace("[Name]", scholar.ScholarshipName).Replace("[Month]", scholar.ScholarshipMonth).Replace("[Year]", scholar.ScholarshipYear);
                if (!string.IsNullOrEmpty(scholar.ScholarshipCriteria))
                    str += "<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Awarded based on [Criteria].</w:t></w:r></w:p>".Replace("[Criteria]", scholar.ScholarshipCriteria);
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }


        public static string GetOverseasContent(OverseasExperience resume, List<OverseasStudy> overseasStudies)
        {
            if (resume.IsOptOut)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GetOverseasStudyContent(overseasStudies));
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        private static string GetOverseasStudyContent(List<OverseasStudy> overseasStudies)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var overseasStudy in overseasStudies)
            {
                if (num > 1 && num <= overseasStudies.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='10'/><w:sz-cs w:val='10'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[CollegeName]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [CountryText]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedDate] - [EndedDate]</w:t></w:r></w:p>".Replace("[CollegeName]", overseasStudy.CollegeName).Replace("[City]", overseasStudy.City).Replace("[CountryText]", overseasStudy.CountryId.ToString()).Replace("[StartedDate]",overseasStudy.StartedDate == null ? "" : Convert.ToDateTime(overseasStudy.StartedDate).ToString("MMMM") + " " + Convert.ToDateTime(overseasStudy.StartedDate).ToString("yyyy")).Replace("[EndedDate]",overseasStudy.EndedDate == null ? "Present" : Convert.ToDateTime(overseasStudy.EndedDate).ToString("MMMM") + " " + Convert.ToDateTime(overseasStudy.EndedDate).ToString("yyyy"));
                stringBuilder.Append(str);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Successfully completed courses in [Classes].</w:t></w:r></w:p>".Replace("[Classes]", overseasStudy.ClassesCompleted));
                string newValue = overseasStudy.LivingSituationId == 3 ? overseasStudy.LivingSituationOther : overseasStudy.LivingSituationId == 2 ? "lived in group housing with other students" : "lived with a family that only spoke the native language";
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[LivingSituation].</w:t></w:r></w:p>".Replace("[LivingSituation]", newValue));
                if (!string.IsNullOrEmpty(overseasStudy.OtherInfo))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[OtherInfo].</w:t></w:r></w:p>".Replace("[OtherInfo]", overseasStudy.OtherInfo));
                ++num;
            }
            return stringBuilder.ToString();
        }



        public static string GetWorkExperienceContent(WorkExperience resume)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<w:p><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Work Experience:</w:t></w:r></w:p>");
            stringBuilder.Append(GetCompanyContent(resume.Companies));
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        private static string GetCompanyContent(List<WorkCompany> companies)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var company in companies)
            {
                if (num > 1 && num <= companies.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='10'/><w:sz-cs w:val='10'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[CompanyName]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[CompanyName]", company.CompanyName).Replace("[City]", company.City).Replace("[State]", company.State).Replace("[StartedMonth]", company.StartMonth).Replace("[StartedYear]", company.StartYear.ToString());
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", company.EndMonth, company.EndYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                stringBuilder.Append(GetCompanyJobContent(company.Positions));
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetCompanyJobContent(List<WorkPosition> jobs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var job in jobs)
            {
                if (num > 1 && num <= jobs.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:i/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[Title]", job.Title).Replace("[StartedMonth]", job.StartMonth).Replace("[StartedYear]", job.StartYear.ToString());
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", job.EndMonth, job.EndYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                stringBuilder.Append(GetJobResponsibilityContent(job));
                stringBuilder.Append(GetJobAwardContent(job.JobAwards));
                if (!string.IsNullOrEmpty(job.Project1))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Selected for special project on [Project].</w:t></w:r></w:p>".Replace("[Project]", job.Project1));
                if (!string.IsNullOrEmpty(job.Project2))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Selected for special project on [Project].</w:t></w:r></w:p>".Replace("[Project]", job.Project2));
                if (job.PercentageImprovement != null)
                {
                    if (!string.IsNullOrEmpty(job.ImproveProductivity) && job.IncreaseRevenue != null)
                    {
                        string str3 = "<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Used skills including [Description] which increased revenue by $[Revenue] and improved productivity by [Productivity] percent.</w:t></w:r></w:p>".Replace("[Description]", job.PercentageImprovement.ToString()).Replace("[Revenue]", job.IncreaseRevenue.ToString()).Replace("[Productivity]", job.ImproveProductivity);
                        stringBuilder.Append(str3);
                    }
                    else if (job.IncreaseRevenue != null)
                    {
                        string str4 = "<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Used skills including [Description] which increased revenue by $[Revenue].</w:t></w:r></w:p>".Replace("[Description]", job.PercentageImprovement.ToString()).Replace("[Revenue]", job.IncreaseRevenue.ToString());
                        stringBuilder.Append(str4);
                    }
                    else
                    {
                        string str5 = "<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Used skills including [Description] which improved productivity by [Productivity] percent.</w:t></w:r></w:p>".Replace("[Description]", job.PercentageImprovement.ToString()).Replace("[Productivity]", job.ImproveProductivity);
                        stringBuilder.Append(str5);
                    }
                }
                ++num;
            }
            return stringBuilder.ToString();
        }

        public static string GetJobResponsibilityContent(WorkPosition job)
        {
            string str = job.JobResponsibilityText;
            var jrList = job.responsibilityOptions;
            var jqList = job.workRespQuestions;
            string otherResp = job.OtherResponsibility;
            switch (str)
            {
                case "Administrative Assistant Responsibilities":
                    return GetAdministrativeAssistantContent(jrList, jqList, otherResp);
                case "Childcare/Camp Counselor Responsibilities":
                    return GetChildcareCampCounselorContent(jrList, jqList, otherResp);
                case "Community or Public Service Responsibilities":
                    return GetCommunityPublicServiceContent(jrList, jqList, otherResp);
                case "Event Planning Responsibilities":
                    return GetEventPlanningContent(jrList, jqList, otherResp);
                case "Financial Organization Responsibilities":
                    return GetFinancialOrganizationContent(jrList, jqList, otherResp);
                case "Marketing Responsibilities":
                    return GetMarketingContent(jrList, jqList, otherResp);
                case "Other":
                    return GetOtherContent(jrList, jqList, otherResp);
                case "Outside Salesperson Responsibilities":
                    return GetOutsideSalespersonContent(jrList, jqList, otherResp);
                case "Political-based Responsibilities":
                    return GetPoliticalBasedContent(jrList, jqList, otherResp);
                case "Public Relations Responsibilities":
                    return GetPublicRelationsContent(jrList, jqList, otherResp);
                case "Research Responsibilities":
                    return GetResearchContent(jrList, jqList, otherResp);
                case "Restaurant Manager Responsibilities":
                    return GetRestaurantManagerContent(jrList, jqList, otherResp);
                case "Retail Manager Responsibilities":
                    return GetRetailManagerContent(jrList, jqList, otherResp);
                case "Retail Salesperson Responsibilities":
                    return GetRetailSalespersonContent(jrList, jqList, otherResp);
                case "Scientific Organization Responsibilities":
                    return GetScientificOrganizationContent(jrList, jqList, otherResp);
                case "Server/Host Responsibilities":
                    return GetServerHostContent(jrList, jqList, otherResp);
                case "Sports Organization Responsibilities":
                    return GetSportsOrganizationContent(jrList, jqList, otherResp);
                case "Teaching Responsibilities":
                    return GetTeachingContent(jrList, jqList, otherResp);
                case "Technically Based Responsiblities":
                    return GetTechnicallyBasedContent(jrList, jqList, otherResp);
                default:
                    return "";
            }
        }

        private static string GetChildcareCampCounselorContent(
          List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 4)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = jr.Caption;
                if (str == "Guide daily activities and play")
                {
                    string newValue = "Guide daily activities and play at [Q1].".Replace("[Q1]", jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate group activities")
                {
                    string newValue = "Coordinate group activities with [Q2] children.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create age appropriate activities")
                {
                    string newValue = "Create age appropriate activities including [Q3].".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage children's safety")
                {
                    string newValue = "Manage children’s safety while planning [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with parents")
                {
                    string newValue = "Communicate with parents regarding behavior and development of [Q2] children.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Maintain daily schedule for children")
                {
                    string newValue = "Maintain daily schedule for [Q2] children.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetAdministrativeAssistantContent(
         List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 6)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            foreach (var jr in jrList)
            {
                string str = jr.Caption;
                if (str == "Interact with customers")
                {
                    string newValue = "Interact with [Q3] customers at a [Q1] organization.".Replace("[Q3]", jqList[2].Answer).Replace("[Q1]", jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate administrative tasks including filing")
                {
                    string newValue = "Coordinate administrative tasks including filing to maximize efficiency for [Q4] departments.".Replace("[Q4]", jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage database")
                {
                    string newValue = "Manage database with information on [Q3] customers.".Replace("[Q3]", jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Schedule appointments")
                {
                    string newValue = "Schedule appointments for [Q2] and [Q4].".Replace("[Q2]", jqList[1].Answer).Replace("[Q4]", jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Plan meetings")
                {
                    string newValue = "Plan meetings including [Q6].".Replace("[Q6]", jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Correspond with customers")
                {
                    string newValue = "Correspond with [Q3] customers for [Q1] company.".Replace("[Q3]", jqList[2].Answer).Replace("[Q1]", jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Purchase office materials")
                {
                    string newValue = "Purchase office materials for [Q4].".Replace("[Q4]", jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Respond to customer requests")
                {
                    string newValue = "Respond to [Q3] customer requests weekly.".Replace("[Q3]", jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Learned new systems or technology")
                {
                    string newValue = "Learned [Q1] new systems or technology.".Replace("[Q1]", jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Report directly to")
                {
                    string newValue = "Report directly to [Q2].".Replace("[Q2]", jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    flag = true;
            }
            if (!string.IsNullOrEmpty(jqList[4].Answer))
            {
                string newValue = "Completed training on [Q5].".Replace("[Q5]", jqList[4].Answer);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
            }
            if (flag)
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            return stringBuilder.ToString();
        }

        private static string GetCommunityPublicServiceContent(
   List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 9)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = jr.Caption;
                if (str == "Serve the public")
                {
                    string newValue = "Serve more than [Q2] members of the public weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Knowledgeable about policies, rules and directives of the organization")
                {
                    string newValue = "Share knowledge of policies, rules and directives of the [Q1] organization.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Work with a team")
                {
                    string newValue = "Work with a team of [Q5] people within the [Q3] departments.".Replace("[Q5]", (string)jqList[4].Answer).Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Motivate and lead a team")
                {
                    string newValue = "Motivate and lead a team of [Q4] for efficiency in the public sector.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Provide continuing education to the public")
                {
                    string newValue = "Provide continuing education to [Q2] members of the public.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Direct others while under stress and duress")
                {
                    string newValue = "Direct others while under stress and duress to improve situation outcomes for the [Q1] organization.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Initiate programs or projects")
                {
                    string newValue = "Initiate programs or projects including [Q8].".Replace("[Q8]", (string)jqList[7].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Provide mentorship to team members in other departments")
                {
                    string newValue = "Provide mentorship to [Q5] team members in other departments.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Learned new systems or technology")
                {
                    string newValue = "Learned [Q6] new systems or technology.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Collaborate with internal department members")
                {
                    string newValue = "Collaborate with [Q5] internal department members.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Report daily or weekly information")
                {
                    string newValue = "Report to [Q7] on community information and internal reports.".Replace("[Q7]", (string)jqList[6].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Completed special training courses")
                {
                    string newValue = "Successfully completed special training courses on [Q9].".Replace("[Q9]", (string)jqList[8].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetEventPlanningContent(
            List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp
            )
        {
            if (jqList == null || jqList.Count < 5)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Manage client requests")
                {
                    string newValue = "Manage client requests for [Q1] company.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create thematic events")
                {
                    string newValue = "Create thematic events while collaborating with [Q2].".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Set up event materials")
                {
                    string newValue = "Set up event materials at events including [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate event day activities")
                {
                    string newValue = "Coordinate event day activities at [Q5] with satisfied client feedback.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Research venue or entertainment information")
                {
                    string newValue = "Research venue or entertainment information for [Q3] clients weekly.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Develop client relationships")
                {
                    string newValue = "Develop client relationships through detailed execution of events such as [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with clients")
                {
                    string newValue = "Communicate with [Q3] clients each week regarding upcoming events.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage database")
                {
                    string newValue = "Manage database with confidential information on [Q3] clients.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Plan specific parts of an event")
                {
                    string newValue = "Plan specific parts of an event including [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Budget event details for clients")
                {
                    string newValue = "Budget event details for clients such as [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetFinancialOrganizationContent(
      List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 5)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Manage database")
                {
                    string newValue = "Manage database involving confidential information for [Q2] clients.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Analyze numerical information")
                {
                    string newValue = "Analyze numerical information and create reports for [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Collaborate with internal office teams")
                {
                    string newValue = "Collaborate with internal office teams on research at [Q1] company.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update internal documents")
                {
                    string newValue = "Update internal documents while collaborating with [Q3] departments.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Interact with clients")
                {
                    string newValue = "Interact with [Q2] clients weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Research information")
                {
                    string newValue = "Research information used by [Q3] for [Q5].".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetMarketingContent(
            List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 6)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Develop database")
                {
                    string newValue = "Develop database with confidential client information for [Q2] customers.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create marketing plan")
                {
                    string newValue = "Create marketing plan for [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Conduct customer surveys")
                {
                    string newValue = "Conduct customer surveys in various venues for a [Q1] company.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Execute focus group research")
                {
                    string newValue = "Execute focus group research as needed for [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create promotional pieces")
                {
                    string newValue = "Create promotional pieces while collaborating with [Q3] departments.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Research competitive market information")
                {
                    string newValue = "Research competitive market information to be used in projects for [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with customers")
                {
                    string newValue = "Communicate with customers regarding [Q6] as planned and executed.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update website")
                {
                    string newValue = "Update website to reflect current business partnerships with [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Prepare presentations")
                {
                    string newValue = "Prepare presentations used by [Q5] in meetings with [Q2] clients.".Replace("[Q5]", (string)jqList[4].Answer).Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetOtherContent(
              List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 3)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty((string)jqList[0].Answer))
            {
                string newValue = "[Q1]".Replace("[Q1]", (string)jqList[0].Answer);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
            }
            if (!string.IsNullOrEmpty((string)jqList[1].Answer))
            {
                string newValue = "[Q2]".Replace("[Q2]", (string)jqList[1].Answer);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
            }
            if (!string.IsNullOrEmpty((string)jqList[2].Answer))
            {
                string newValue = "[Q3]".Replace("[Q3]", (string)jqList[2].Answer);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
            }
            return stringBuilder.ToString();
        }

        private static string GetOutsideSalespersonContent(
     List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 6)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Negotiate product placement")
                {
                    string newValue = "Negotiate product placement for [Q1] company in retail stores.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create visual displays")
                {
                    string newValue = "Create visual displays with [Q3] products weekly.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Merchandise product lines")
                {
                    string newValue = "Merchandise product lines that include [Q3] items.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage territory database information")
                {
                    string newValue = "Manage territory database information for [Q2] customers.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Report weekly activity")
                {
                    string newValue = "Report weekly activity for a sales territory generating $[Q4] in monthly revenue.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Responsible for merchandising, sales, and account development")
                {
                    string newValue = "Responsible for merchandising, sales, and account development in [Q2] customer outlets.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Develop relationships with accounts")
                {
                    string newValue = "Develop relationships with [Q2] customer accounts through demos of [Q6].".Replace("[Q2]", (string)jqList[1].Answer).Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate product features and benefits to customers")
                {
                    string newValue = "Communicate product features and benefits to [Q2] customers resulting in $[Q4] in revenue.".Replace("[Q2]", (string)jqList[1].Answer).Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Increase sales through promotion of new products")
                {
                    string newValue = "Increase sales through promotion of new products including a line of [Q3] items.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Launch new products")
                {
                    string newValue = "Launch new products and participate in [Q5] training programs ongoing.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetPoliticalBasedContent(
             List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 4)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Manage database")
                {
                    string newValue = "Manage database with confidential information reported to [Q2].".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate event activities")
                {
                    string newValue = "Coordinate event activities such as [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Prepare informational materials")
                {
                    string newValue = "Prepare informational materials including research for [Q3].".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Maintain relationships with constituency")
                {
                    string newValue = "Maintain relationships with [Q3] constituency.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Write news updates")
                {
                    string newValue = "Write news updates for publication on internal websites regarding activities of [Q2].".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Collaborate with internal office teams")
                {
                    string newValue = "Collaborate with [Q1] internal office teams.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Research political transcript information, party policy and interests")
                {
                    string newValue = "Research political transcript information, party policy and interests for distribution to [Q1].".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Track media information")
                {
                    string newValue = "Track media information and report data to [Q2] for presentation purposes.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }


        private static string GetPublicRelationsContent(
            List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 8)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Write press releases")
                {
                    string newValue = "Write press releases for [Q7] at a [Q1] company.".Replace("[Q7]", (string)jqList[6].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Construct media kits")
                {
                    string newValue = "Construct media kits for [Q6] monthly.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage client database")
                {
                    string newValue = "Manage client database with over [Q3] clients’ confidential information.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Develop client relationships")
                {
                    string newValue = "Develop client relationships while interacting with [Q4] team members.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Develop media lists")
                {
                    string newValue = "Develop media lists for [Q3] clients to be used by [Q2].".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with media contacts")
                {
                    string newValue = "Communicate with media contacts via phone and email about projects from [Q5] departments.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Compile articles")
                {
                    string newValue = "Compile articles used in [Q6] media kits and [Q7] press releases.".Replace("[Q6]", (string)jqList[5].Answer).Replace("[Q7]", (string)jqList[6].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update website")
                {
                    string newValue = "Update website with information from [Q2] clients’ portfolios.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Learn database systems (Cision, ProfNet, PRNewswire)")
                {
                    string newValue = "Learn database systems (Cision, ProfNet, PRNewswire) by successfully completing [Q8] training.".Replace("[Q8]", (string)jqList[7].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetResearchContent(
            List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 4)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Develop database")
                {
                    string newValue = "Develop database for clients in a [Q1] company.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Conduct customer surveys")
                {
                    string newValue = "Conduct customer surveys with information from [Q2] departments for clients.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Research competitive market information")
                {
                    string newValue = "Research competitive market information used for [Q4] given to [Q3].".Replace("[Q4]", (string)jqList[3].Answer).Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with internal management")
                {
                    string newValue = "Communicate with internal management on progress including research for [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Collaborate with other departments on projects")
                {
                    string newValue = "Collaborate with [Q2] departments on projects.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update website")
                {
                    string newValue = "Update website to increase new business within [Q1] company.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Prepare presentations")
                {
                    string newValue = "Prepare presentations regarding ongoing results from research for [Q3].".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }


        private static string GetRestaurantManagerContent(
          List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 7)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Encourage open communication with kitchen and serving staff")
                {
                    string newValue = "Encourage open communication with [Q3] kitchen and serving staff at [Q1] restaurant.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Schedule wait staff")
                {
                    string newValue = "Schedule [Q3] wait staff to manage [Q2] customers weekly.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Schedule kitchen staff")
                {
                    string newValue = "Schedule [Q3] kitchen staff for service generating $[Q4] weekly.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate reservations and customer turn rate")
                {
                    string newValue = "Coordinate reservations and customer turn rate for [Q2] customers at [Q1] restaurant.".Replace("[Q2]", (string)jqList[1].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Responsible for opening and closing")
                {
                    string newValue = "Responsible for opening and closing in addition to cash and credit transactions totaling $[Q4] weekly.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Supervise team of employees")
                {
                    string newValue = "Supervise team of [Q3] employees at [Q1] restaurant.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Train new employees")
                {
                    string newValue = "Train [Q7] new employees on [Q6].".Replace("[Q7]", (string)jqList[6].Answer).Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Set sales goals")
                {
                    string newValue = "Set sales goals which generate $[Q4] in weekly revenue.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Interview potential new employees")
                {
                    string newValue = "Interview potential new employees and train on [Q6] ongoing.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Conduct performance reviews for employees")
                {
                    string newValue = "Conduct performance reviews for [Q3] employees.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Direct daily operations for wait and kitchen staff")
                {
                    string newValue = "Direct daily operations for [Q3] wait and kitchen staff.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    flag = true;
            }
            if (!string.IsNullOrEmpty((string)jqList[4].Answer))
            {
                string newValue = "Completed training on [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
            }
            if (flag)
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            return stringBuilder.ToString();
        }

        private static string GetRetailManagerContent(
            List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 7)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Coordinate sales and merchandising activities")
                {
                    string newValue = "Coordinate sales and merchandising activities for [Q3] employees at a [Q1] store.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Responsible for opening and closing")
                {
                    string newValue = "Responsible for opening and closing at a store with $[Q4] in weekly revenue.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Supervise team of employees")
                {
                    string newValue = "Supervise team of [Q3] employees for sales to [Q2] customers weekly.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Train new employees")
                {
                    string newValue = "Train [Q7] new employees on [Q6].".Replace("[Q7]", (string)jqList[6].Answer).Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Set sales goals")
                {
                    string newValue = "Set sales goals for [Q3] salespeople resulting in $[Q4] weekly.".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Interview potential new employees")
                {
                    string newValue = "Interview potential new employees and implement [Q6] training.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Conduct performance reviews for employees")
                {
                    string newValue = "Conduct performance reviews for [Q3] employees annually.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Direct store personnel on daily operations")
                {
                    string newValue = "Direct store personnel on daily operations while completing [Q5] training courses.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetRetailSalespersonContent(
     List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 8)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Create positive environment for customers")
                {
                    string newValue = "Create positive environment for [Q2] customers in a [Q1] store.".Replace("[Q2]", (string)jqList[1].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Merchandise product lines on floor")
                {
                    string newValue = "Merchandise [Q4] product lines on floor to maximize sales.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create window and in-store displays")
                {
                    string newValue = "Create window and in-store displays [Q5] which increase store traffic.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Assist with fitting room management")
                {
                    string newValue = "Assist with fitting room management for up to [Q2] customers weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Increase revenue through promotion of additional items to customers")
                {
                    string newValue = "Increase revenue through promotion of additional items to customers generating $[Q3] in weekly sales.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate merchandise with customer needs")
                {
                    string newValue = "Coordinate merchandise with customer needs while managing inventory for [Q4] products.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage daily cash register balances")
                {
                    string newValue = "Manage daily cash register balances for transactions with [Q2] customers.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Promote special items")
                {
                    string newValue = "Promote special items and recommend [Q4] basic line items.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Trained new sales personnel")
                {
                    string newValue = "Trained [Q7] new sales personnel on [Q6]. ".Replace("[Q7]", (string)jqList[6].Answer).Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Develop clients")
                {
                    string newValue = "Develop clients and successfully completed [Q8] training.".Replace("[Q8]", (string)jqList[7].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetScientificOrganizationContent(
    List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 5)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Analyze numerical information")
                {
                    string newValue = "Analyze numerical information within a [Q1] company.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Collaborate with internal office teams")
                {
                    string newValue = "Collaborate with internal office teams for projects for [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update internal documents")
                {
                    string newValue = "Update internal documents as needed for [Q2] clients.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Interact with clients")
                {
                    string newValue = "Interact with [Q2] clients weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage client information")
                {
                    string newValue = "Manage client information for internal distribution to [Q3] departments.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Research information")
                {
                    string newValue = "Research information on [Q5] to be used with existing clients.".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetServerHostContent(
            List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 7)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Create positive social environment for customers")
                {
                    string newValue = "Create positive social environment for customers at [Q1] restaurant.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage customer flow")
                {
                    string newValue = "Manage customer flow to maximize experience for [Q2] customers weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Organize seating arrangement in dining area")
                {
                    string newValue = "Organize seating arrangement in dining area for [Q2] customers weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Assist with banquet room service")
                {
                    string newValue = "Assist with banquet room service at [Q1] restaurant with [Q5] employees.".Replace("[Q1]", (string)jqList[0].Answer).Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Increase revenue through promotion of additional items to customers")
                {
                    string newValue = "Increase revenue through promotion of [Q6] items to customers weekly.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate activities of serving team")
                {
                    string newValue = "Coordinate activities of [Q5] serving team members at [Q1] restaurant.".Replace("[Q1]", (string)jqList[0].Answer).Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage daily cash flow")
                {
                    string newValue = "Manage cash and credit transactions of up to $[Q7] weekly.".Replace("[Q7]", (string)jqList[6].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Promote special menu items")
                {
                    string newValue = "Promote special menu items in addition to [Q6] regular items.".Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Trained new staff members")
                {
                    string newValue = "Trained [Q5] new staff members on [Q4].".Replace("[Q5]", (string)jqList[4].Answer).Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate restaurant information via telephone to customers")
                {
                    string newValue = "Communicate restaurant information via telephone to [Q2] customers weekly.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    flag = true;
            }
            if (!string.IsNullOrEmpty((string)jqList[2].Answer))
            {
                string newValue = "Completed training on [Q3].".Replace("[Q3]", (string)jqList[2].Answer);
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
            }
            if (flag)
                stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            return stringBuilder.ToString();
        }

        private static string GetSportsOrganizationContent(
    List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 5)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Manage database")
                {
                    string newValue = "Manage database with information about [Q2] clients.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate event activities")
                {
                    string newValue = "Coordinate event activities such as [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Maintain relationships with fans")
                {
                    string newValue = "Maintain relationships with fans while reporting to [Q1].".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Write news updates on team")
                {
                    string newValue = "Write news updates on team for distribution to [Q3] and media outlets.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update website information")
                {
                    string newValue = "Update website information to reflect current status with [Q3].".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate game day activities")
                {
                    string newValue = "Coordinate gameday activities including [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Execute promotions during games")
                {
                    string newValue = "Execute promotions during games [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with sponsors")
                {
                    string newValue = "Communicate with sponsors via email and phone to update them on [Q4] ongoing.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetTeachingContent(
                List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 6)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Guide daily activities and play")
                {
                    string newValue = "Guide daily activities and play for [Q2] students in a [Q1] school.".Replace("[Q2]", (string)jqList[1].Answer).Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Update student records")
                {
                    string newValue = "Update [Q2] student records while participating in [Q6].".Replace("[Q2]", (string)jqList[1].Answer).Replace("[Q6]", (string)jqList[5].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Coordinate group activities")
                {
                    string newValue = "Coordinate group activities including [Q4].".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Create age appropriate activities")
                {
                    string newValue = "Create age appropriate activities for [Q2] students to interact safely.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Implement safety procedures")
                {
                    string newValue = "Implement safety procedures as directed by state regulations for [Q1] school.".Replace("[Q1]", (string)jqList[0].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Communicate with parents")
                {
                    string newValue = "Communicate with parents of [Q2] students on progress and development opportunities.".Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Establish classroom expectations")
                {
                    string newValue = "Establish classroom expectations while teaching [Q3] and working on [Q5].".Replace("[Q3]", (string)jqList[2].Answer).Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Develop curriculum")
                {
                    string newValue = "Develop curriculum for [Q3] ongoing.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetTechnicallyBasedContent(
  List<ResponsibilityOptionsResponse> jrList,
         List<WorkRespQuestion> jqList,
         string otherResp)
        {
            if (jqList == null || jqList.Count < 9)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var jr in jrList)
            {
                string str = (string)jr.Caption;
                if (str == "Install equipment")
                {
                    string newValue = "Install [Q5] equipment for [Q2] customers weekly.".Replace("[Q5]", (string)jqList[4].Answer).Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Repair machinery or systems")
                {
                    string newValue = "Repair machinery or systems including [Q5].".Replace("[Q5]", (string)jqList[4].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Upgrade company systems")
                {
                    string newValue = "Upgrade company systems such as [Q7].".Replace("[Q7]", (string)jqList[6].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Analyze ways to improve programs")
                {
                    string newValue = "Analyze ways to improve programs such as [Q8].".Replace("[Q8]", (string)jqList[7].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Work on a team")
                {
                    string newValue = "Work on a team at a [Q1] organization while interacting with [Q2] customers per week.".Replace("[Q1]", (string)jqList[0].Answer).Replace("[Q2]", (string)jqList[1].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Manage people")
                {
                    string newValue = "Manage [Q4] people for technical projects.".Replace("[Q4]", (string)jqList[3].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Control inventory")
                {
                    string newValue = "Control inventory of more than [Q3] items.".Replace("[Q3]", (string)jqList[2].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Completed special training courses")
                {
                    string newValue = "Successfully completed special training courses in [Q6] and [Q9].".Replace("[Q6]", (string)jqList[5].Answer).Replace("[Q9]", (string)jqList[8].Answer);
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", newValue));
                }
                if (str == "Other")
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value]</w:t></w:r></w:p>".Replace("[Value]", otherResp));
            }
            return stringBuilder.ToString();
        }

        private static string GetJobAwardContent(List<JobAward> awards)
        {
            if (awards == null || awards.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var award in awards)
            {
                string str = "<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>Awarded [Award] in [Month] [Year] for excellent performance.</w:t></w:r></w:p>".Replace("[Award]", award.AwardDesc).Replace("[Month]", award.AwardedMonth).Replace("[Year]", award.AwardedYear);
                stringBuilder.Append(str);
            }
            return stringBuilder.ToString();
        }

        public static string GetMilitaryExperienceContent(MilitaryExperience resume, List<MilitaryPosition> positions)
        {
            if (resume == null)
                return "";
            if (resume.IsOptOut)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<w:p><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Military Experience:</w:t></w:r></w:p>");
            stringBuilder.Append("<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Branch]</w:t></w:r><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t> [Rank]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [Country]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedMonth] [EndedYear]</w:t></w:r></w:p>".Replace("[Branch]", resume.Branch).Replace("[Rank]", resume.Rank).Replace("[City]", resume.City).Replace("[Country]", resume.CountryName).Replace("[StartedMonth]", resume.StartedMonth).Replace("[StartedYear]", resume.StartedYear).Replace("[EndedMonth]", resume.EndedMonth).Replace("[EndedYear]", resume.EndedYear));
            stringBuilder.Append(GetMilitaryPositionContent(positions));
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        private static string GetMilitaryPositionContent(List<MilitaryPosition> positions)
        {
            if (positions == null || positions.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var position in positions)
            {
                if (num > 1 && num <= positions.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:i/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedMonth] [EndedYear]</w:t></w:r></w:p>".Replace("[Title]", position.Title).Replace("[StartedMonth]", position.StartedMonth).Replace("[StartedYear]", position.StartedYear).Replace("[EndedMonth]", position.EndedMonth).Replace("[EndedYear]", position.EndedYear);
                stringBuilder.Append(str);
                if (!string.IsNullOrEmpty(position.MainTraining))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.MainTraining));
                if (!string.IsNullOrEmpty(position.Responsibility1))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility1));
                if (!string.IsNullOrEmpty(position.Responsibility2))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility2));
                if (!string.IsNullOrEmpty((position.Responsibility3)))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility3));
                if (!string.IsNullOrEmpty(position.OtherInfo))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.OtherInfo));
                ++num;
            }
            return stringBuilder.ToString();
        }


        public static string GetOrgAndVolunteerExperienceContent(OrgExperience orgExperience, List<Organization> organizations, List<OrgPosition> orgPositions, VolunteerExperience volunteerExperience, List<VolunteerOrg> volunteerOrg, List<VolunteerPosition> volunteerPositions)
        {
            //JToken jtoken1 = JObject.Parse(resume.Data)["orgs"];
            //oken jtoken2 = JObject.Parse(resume.Data)["communityServices"];
            if (orgExperience.IsOptOut && volunteerExperience.IsOptOut || organizations.Count < 1 && volunteerOrg.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            string str = "";
            if (!orgExperience.IsOptOut && organizations.Count > 0)
                str = "Organization";
            if (!(bool)orgExperience.IsOptOut && organizations.Count> 0 && !volunteerExperience.IsOptOut && volunteerOrg.Count > 0)
                str += " and ";
            if (!(bool)volunteerExperience.IsOptOut && volunteerOrg.Count > 0)
                str += "Volunteer";
            string newValue = str + " Experience:";
            stringBuilder.Append("<w:p><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>[Heading]</w:t></w:r></w:p>".Replace("[Heading]", newValue));
            if (!(bool)orgExperience.IsOptOut && organizations.Count > 0)
                stringBuilder.Append(GetOrganizationContent(organizations, orgPositions));
            if (!(bool)volunteerExperience.IsOptOut && volunteerOrg.Count > 0)
            {
                if (!(bool)orgExperience.IsOptOut && organizations.Count > 0)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                stringBuilder.Append(GetVolunteerOrgContent(volunteerOrg, volunteerPositions));
            }
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

       
        private static string GetOrganizationContent(List<Organization> orgs, List<OrgPosition> positions)
        {
            if (orgs == null || orgs.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var org in orgs)
            {
                if (num > 1 && num <= orgs.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='10'/><w:sz-cs w:val='10'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[OrgName]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[OrgName]", org.OrgName).Replace("[City]", org.City).Replace("[State]", org.StateName).Replace("[StartedMonth]", org.StartedMonth).Replace("[StartedYear]", org.StartedYear);
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", org.EndedMonth, org.EndedYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                stringBuilder.Append(GetOrgPositionContent(positions));
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetOrgPositionContent(List<OrgPosition> positions)
        {
            if (positions == null || positions.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var position in positions)
            {
                if (num > 1 && num <= positions.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:i/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[Title]", position.Title).Replace("[StartedMonth]", position.StartedMonth).Replace("[StartedYear]", position.StartedYear);
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", position.EndedMonth, position.EndedYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                if (!string.IsNullOrEmpty(position.Responsibility1))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility1));
                if (!string.IsNullOrEmpty(position.Responsibility2))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility2));
                if (!string.IsNullOrEmpty(position.Responsibility3))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility3));
                if (!string.IsNullOrEmpty(position.OtherInfo))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.OtherInfo));
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetVolunteerOrgContent(List<VolunteerOrg> orgs, List<VolunteerPosition> positions)
        {
            if (orgs == null || orgs.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var org in orgs)
            {
                if (num > 1 && num <= orgs.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='10'/><w:sz-cs w:val='10'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[OrgName]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[OrgName]", org.VolunteerOrg1).Replace("[City]", org.City).Replace("[State]", org.StateName).Replace("[StartedMonth]", org.StartedMonth).Replace("[StartedYear]", org.StartedYear);
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", org.EndedMonth, org.EndedYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                stringBuilder.Append(GetVolunteerPositionContent(positions));
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetVolunteerPositionContent(List<VolunteerPosition> positions)
        {
            if (positions == null || positions.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var position in positions)
            {
                if (num > 1 && num <= positions.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:i/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[Title]", position.Title).Replace("[StartedMonth]", position.StartedMonth).Replace("[StartedYear]", position.StartedYear);
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", position.EndedMonth, position.EndedYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                if (!string.IsNullOrEmpty(position.Responsibility1))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility1));
                if (!string.IsNullOrEmpty(position.Responsibility2))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility2));
                if (!string.IsNullOrEmpty(position.Responsibility3))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility3));
                if (!string.IsNullOrEmpty(position.OtherInfo))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.OtherInfo));
                ++num;
            }
            return stringBuilder.ToString();
        }


        public static string GetProfessionalContent(ResumeGenerateModel resume)
        {
            //JToken jtoken = JObject.Parse(resume.Data)["lcas"];
            if (resume.Professional.IsOptOut || resume.Licenses.Count < 1 && resume.Certificates.Count < 1 && resume.Affiliations.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            if (resume.Licenses != null && resume.Licenses.Count > 0 || resume.Certificates != null && resume.Certificates.Count > 0)
            {
                stringBuilder.Append("<w:p><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Licenses and Certificates:</w:t></w:r></w:p>");
                if (resume.Licenses != null && resume.Licenses.Count> 0)
                    stringBuilder.Append(GetLicenseContent(resume.Licenses));
                if (resume.Certificates != null && resume.Certificates.Count > 0)
                {
                    if (resume.Licenses != null && resume.Licenses.Count > 0)
                        stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                    stringBuilder.Append(GetCertificateContent(resume.Certificates));
                }
            }
            if (resume.Affiliations != null && resume.Affiliations.Count > 0)
            {
                stringBuilder.Append("<w:p><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Professional Affiliations:</w:t></w:r></w:p>");
                stringBuilder.Append(GetAffiliationContent(resume.Affiliations));
            }
            stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

        private static string GetLicenseContent(List<License> lics)
        {
            if (lics == null || lics.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var lic in lics)
            {
                if (num > 1 && num <= lics.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[Month] [Year]</w:t></w:r></w:p>".Replace("[Title]", lic.Title).Replace("[State]", lic.StateName).Replace("[Month]", lic.ReceivedMonth).Replace("[Year]", lic.ReceivedYear);
                stringBuilder.Append(str);
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetCertificateContent(List<Certificate> certs)
        {
            if (certs == null || certs.Count< 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var cert in certs)
            {
                if (num > 1 && num <= certs.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[Month] [Year]</w:t></w:r></w:p>".Replace("[Title]", cert.Title).Replace("[State]", cert.StateName).Replace("[Month]", cert.ReceivedMonth).Replace("[Year]", cert.ReceivedYear);
                stringBuilder.Append(str);
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetAffiliationContent(List<Affiliation> orgs)
        {
            if (orgs == null || orgs.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var org in orgs)
            {
                if (num > 1 && num <= orgs.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='10'/><w:sz-cs w:val='10'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='center' w:pos='5400'/><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[OrgName]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[City], [State]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[OrgName]", org.AffiliationName).Replace("[City]", org.City).Replace("[State]", org.StateName).Replace("[StartedMonth]", org.StartedMonth).Replace("[StartedYear]", org.StartedYear);
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}", org.EndedMonth, org.EndedYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                stringBuilder.Append(GetOrgPositionContent(org.AffiliationPositions));
                ++num;
            }
            return stringBuilder.ToString();
        }

        private static string GetOrgPositionContent(List<AffiliationPosition> positions)
        {
            if (positions == null || positions.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            foreach (var position in positions)
            {
                if (num > 1 && num <= positions.Count)
                    stringBuilder.Append("<w:p><w:pPr><w:rPr><w:sz w:val='6'/><w:sz-cs w:val='6'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>");
                string str1 = "<w:p><w:pPr><w:tabs><w:tab w:val='right' w:pos='10800'/></w:tabs><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:i/><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Title]</w:t></w:r><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:tab/><w:t>[StartedMonth] [StartedYear] - [EndedDate]</w:t></w:r></w:p>".Replace("[Title]", position.Title).Replace("[StartedMonth]", position.StartedMonth).Replace("[StartedYear]", position.StartedYear);
                string str2 = !false ? str1.Replace("[EndedDate]", string.Format("{0} {1}",position.EndedMonth, position.EndedYear)) : str1.Replace("[EndedDate]", "Present");
                stringBuilder.Append(str2);
                if (!string.IsNullOrEmpty(position.Responsibility1))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility1));
                if (!string.IsNullOrEmpty(position.Responsibility2))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility2));
                if (!string.IsNullOrEmpty(position.Responsibility3))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.Responsibility3));
                if (!string.IsNullOrEmpty(position.OtherInfo))
                    stringBuilder.Append("<w:p><w:pPr><w:listPr><w:ilvl w:val='0'/><w:ilfo w:val='5'/><wx:t wx:val='·' wx:wTabBefore='0' wx:wTabAfter='270'/><wx:font wx:val='Symbol'/></w:listPr><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr></w:pPr><w:r><w:rPr><w:sz w:val='22'/><w:sz-cs w:val='22'/></w:rPr><w:t>[Value].</w:t></w:r></w:p>".Replace("[Value]", position.OtherInfo));
                ++num;
            }
            return stringBuilder.ToString();
        }

        public static string GetTechnicalContent(ResumeGenerateModel resume)
        {
            string skills = "";
            skills += resume.TechnicalSkill.Msword.Value ? "Microsoft Word, " : "";
            skills += resume.TechnicalSkill.Msexcel.Value ? "Microsoft Excel, " : "";
            skills += resume.TechnicalSkill.MspowerPoint.Value ? "Microsoft Powerpoint, " : "";
            skills += resume.TechnicalSkill.Msoutlook.Value ? "Microsoft Outlook, " : "";
            skills += resume.TechnicalSkill.MacPages.Value ? "Macintosh Pages, " : "";
            skills += resume.TechnicalSkill.MacNumbers.Value ? "Macintosh Numbers, " : "";
            skills += resume.TechnicalSkill.MacKeynote.Value ? "Macintosh Notes, " : "";
            skills += resume.TechnicalSkill.AdobeAcrobat.Value ? "Adobe Acrobat, " : "";
            skills += resume.TechnicalSkill.AdobePublisher.Value ? "Adobe Publisher, " : "";
            skills += resume.TechnicalSkill.AdobeIllustrator.Value ? "Adobe Illustrator, " : "";
            skills += resume.TechnicalSkill.AdobePhotoshop.Value ? "Adobe Photoshop, " : "";
            skills += resume.TechnicalSkill.GoogleSuite.Value ? "Google Suit, " : "";
            skills += resume.TechnicalSkill.GoogleDocs.Value ? "Google Docs" : "";
            return "<w:p><w:pPr><w:ind w:left='1800' w:hanging='1800'/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Computer Skills:</w:t></w:r><w:r><w:rPr><w:b/></w:rPr><w:tab wx:wTab='225' wx:tlc='none' wx:cTlc='3'/></w:r><w:r><w:rPr><w:sz w:val='22'/></w:rPr><w:t>[Skills]</w:t></w:r></w:p>".Replace("[Skills]", skills.ToString()) + "<w:p><w:pPr><w:rPr><w:sz w:val='12'/><w:sz-cs w:val='12'/></w:rPr></w:pPr><w:r><w:t>  </w:t></w:r></w:p>";
        }

        public static string GetLanguageContent(ResumeGenerateModel resume)
        {
            if (resume.LanguageSkill.IsOptOut || resume.Languages == null || resume.Languages.Count < 1)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<w:p><w:pPr><w:ind w:left='1800' w:hanging='1800'/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val='24'/><w:sz-cs w:val='24'/></w:rPr><w:t>Language Skills:</w:t></w:r><w:r><w:rPr><w:b/></w:rPr><w:tab wx:wTab='225' wx:tlc='none' wx:cTlc='3'/></w:r><w:r><w:rPr><w:sz w:val='22'/></w:rPr><w:t>");
            int num = 1;
            foreach (var jtoken2 in resume.Languages)
            {
                if (num > 1)
                    stringBuilder.Append(", ");
                string str = "[Ability] in [Language]".Replace("[Ability]", jtoken2.LanguageAbilityDesc).Replace("[Language]", jtoken2.LanguageName);
                stringBuilder.Append(str);
                ++num;
            }
            stringBuilder.Append("</w:t></w:r></w:p>");
            return stringBuilder.ToString();
        }

    }
}
