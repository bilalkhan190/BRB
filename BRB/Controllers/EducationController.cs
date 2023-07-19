using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services;
using BusinessObjects.Services.interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BRB.Controllers
{
    public class EducationController : BaseController
    {

        private readonly IDropdownService _dropdownService;
        private readonly IResumeService _resumeService;
        private readonly IUserProfileService _userProfileService;
        public EducationController(IDropdownService dropdownService, IResumeService resumeService, IUserProfileService userProfileService)
        {

            _dropdownService = dropdownService;
            _resumeService = resumeService;
            _userProfileService = userProfileService;
        }
      
        public IActionResult GetCollegeDataById(int id) 
        {
            AjaxResponse ajaxResponse= new AjaxResponse();
            var record = (from c in _dbContext.Colleges
                          where c.CollegeId == id
                          select new College
                          {
                              CollegeCity = c.CollegeCity,
                              CollegeId = c.CollegeId,
                              CollegeName = c.CollegeName,
                              CollegeStateAbbr = c.CollegeStateAbbr,
                              CertificateId = c.CertificateId,
                              CertificateOther = c.CertificateOther,
                              IsComplete = c.IsComplete,
                              DegreeId = c.DegreeId,
                              DegreeOther = c.DegreeOther,
                              Gpa = c.Gpa,
                              EducationId = c.EducationId,
                              HonorProgram = c.HonorProgram,
                              GradDate = c.GradDate,
                              IncludeGpa = c.IncludeGpa,
                              MajorId = c.MajorId,
                              MajorOther = c.MajorOther,
                              MajorSpecialtyId = c.MajorSpecialtyId,
                              MajorSpecialtyOther = c.MajorSpecialtyOther,
                              MinorId = c.MinorId,
                              SchoolName = c.SchoolName,
                              MinorOther = c.MinorOther
                          }).FirstOrDefault();

            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetAcademicHonorById(int id)
        {
            AjaxResponse ajaxResponse= new AjaxResponse();
            var record = _dbContext.AcademicHonors.Where(x => x.AcademicHonorId == id).ToList();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);  
        }

        public IActionResult GetAcademicScholarshipById(int id)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            var record = _dbContext.AcademicScholarships.Where(x => x.AcademicScholarshipId == id).ToList();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            TableIdentities ids = new TableIdentities();
            List<EducationViewObject> ListOfObjs = new List<EducationViewObject>();
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            EducationViewObject educationObj = new EducationViewObject();
            var data = _dbContext.Educations.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (data != null)
            {
                var colleges = GetColleges(data.EducationId);
                if (colleges.Count > 0)
                {
                    foreach (var c in colleges)
                    {

                        educationObj.Education = data;
                        educationObj.College = c;
                        educationObj.AcademicHonors = GetAcademicHonors(c.CollegeId);
                        educationObj.AcademicScholarships = GetAcademicScholarship(c.CollegeId);
                        ListOfObjs.Add(educationObj);
                    }
                    ajaxResponse.Data = ListOfObjs;
                }
                else
                {
                    ajaxResponse.Data = data;
                }
            }
            else
            {
                ajaxResponse.Data = null;
            }
          

          
            return Json(ajaxResponse);

        }


        [NonAction]
        private List<College> GetColleges(int educationId)
        {
            var data = (from c in _dbContext.Colleges
                        where c.EducationId == educationId
                        select new College
                        {
                            CollegeCity = c.CollegeCity,
                            CollegeId = c.CollegeId,
                            CollegeName = c.CollegeName,
                            CollegeStateAbbr = c.CollegeStateAbbr,
                            CertificateId = c.CertificateId,
                            CertificateOther = c.CertificateOther,
                            IsComplete = c.IsComplete,
                            DegreeId = c.DegreeId,
                            DegreeOther = c.DegreeOther,
                            Gpa = c.Gpa,
                            EducationId = educationId,
                            HonorProgram = c.HonorProgram,
                            GradDate = c.GradDate,
                            IncludeGpa = c.IncludeGpa,
                            MajorId = c.MajorId,
                            MajorOther = c.MajorOther,
                            MajorSpecialtyId = c.MajorSpecialtyId,
                            MajorSpecialtyOther = c.MajorSpecialtyOther,
                            MinorId = c.MinorId,
                            SchoolName = c.SchoolName,
                            MinorOther = c.MinorOther
                        }).ToList();
            return data;
        }
        [NonAction]
        private List<AcademicHonor> GetAcademicHonors(int collegeId)
        {
            var data = (from ah in _dbContext.AcademicHonors
                        where ah.CollegeId == collegeId
                        select ah).ToList();
            return data;
        }

        [NonAction]
        private List<AcademicScholarship> GetAcademicScholarship(int collegeId)
        {
            var data = (from schlr in _dbContext.AcademicScholarships
                        where schlr.CollegeId == collegeId
                        select schlr).ToList();
            return data;
        }

        [HttpPost]
        public IActionResult PostEducationData(EducationViewModel educationViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Message = "Ops record is not saved we are facing problem";
            ajaxResponse.Success = false;
            ajaxResponse.Redirect = "";
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            Resume resumeProfileData = new Resume();
            resumeProfileData.ResumeId = sessionData.ResumeId;
            resumeProfileData.UserId = sessionData.UserId;
            resumeProfileData.LastSectionVisitedId = educationViewModel.LastSectionVisitedId;
            resumeProfileData.LastModDate = DateTime.Today;
            resumeProfileData.CreatedDate = DateTime.Today;
            resumeProfileData.LastSectionCompletedId = educationViewModel.IsComplete == true ? educationViewModel.LastSectionVisitedId : 0;
            Education education = new Education();
            education.EducationId = ids.educationId;
            education.ResumeId = sessionData.ResumeId;
            education.IsComplete = educationViewModel.IsComplete == true ? educationViewModel.IsComplete : false;
            education.CreatedDate = DateTime.Today;
            education.LastModDate = DateTime.Today;
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    bool masterRecord = _dbContext.Educations.Any(x => x.EducationId == ids.educationId);
                    if (!masterRecord)
                    {
                        _dbContext.Educations.Add(education);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        _dbContext.Educations.Update(education);
                        _dbContext.SaveChanges();
                    }
                    ajaxResponse.Redirect = "/Resume/OverseasStudy";
                    if (educationViewModel.College.Count > 0)
                    {
                        foreach (var college in educationViewModel.College)
                        {
                            college.EducationId = education.EducationId;
                            college.CreatedDate = DateTime.Today;
                            college.LastModDate = DateTime.Today;
                            if (college.EducationId > 0)
                            {
                                _dbContext.Colleges.Update(college);
                                _dbContext.SaveChanges();
                            }
                            else
                            {
                                _dbContext.Colleges.Add(college);
                                _dbContext.SaveChanges();
                            }
                            if (educationViewModel.AcademicHonors.Count > 0)
                            {
                                foreach (var honor in educationViewModel.AcademicHonors)
                                {
                                    honor.CollegeId = college.CollegeId;
                                    honor.CreatedDate = DateTime.Today;
                                    honor.LastModDate = DateTime.Today;
                                    if (honor.CollegeId > 0)
                                    {
                                        _dbContext.AcademicHonors.Update(honor);
                                    }
                                    else
                                    {
                                        _dbContext.AcademicHonors.Add(honor);
                                    }
                                  
                                }
                            }
                            if (educationViewModel.AcademicScholarships.Count > 0)
                            {
                                foreach (var scholarship in educationViewModel.AcademicScholarships)
                                {
                                    scholarship.CollegeId = college.CollegeId;
                                    scholarship.CreatedDate = DateTime.Today;
                                    scholarship.LastModDate = DateTime.Today;
                                    if (scholarship.CollegeId > 0)
                                    {
                                        _dbContext.AcademicScholarships.Update(scholarship);
                                    }
                                    else
                                    {
                                        _dbContext.AcademicScholarships.Add(scholarship);
                                    }
                                }
                            }
                        }
                      
                        ajaxResponse.Data = educationViewModel;
                       
                    }
                    _dbContext.Resumes.Update(resumeProfileData);
                    _dbContext.SaveChanges();
                    transection.Commit();

                }
                catch (Exception ex)
                {
                    transection.Rollback();

                    throw;
                }




            }

            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult DeleteCollege(int id, int academicId, int scholarshipid)
        {
            AjaxResponse ajaxResponse = new AjaxResponse(); 
           
            if (id > 0 && academicId == 0 && scholarshipid == 0)
            {
                var record = _dbContext.Colleges.FirstOrDefault(c => c.CollegeId == id);
                if (record != null) {
                    var academicRecord = _dbContext.AcademicHonors.Where(c => c.CollegeId == record.CollegeId).ToList();
                    var scholarshipRecord = _dbContext.AcademicScholarships.Where(c => c.CollegeId == record.CollegeId).ToList();

                    if (academicRecord.Count > 0)
                    {
                        _dbContext.AcademicHonors.RemoveRange(academicRecord);
                        _dbContext.SaveChanges();

                    }


                    if (scholarshipRecord.Count > 0)
                    {
                        _dbContext.AcademicScholarships.RemoveRange(scholarshipRecord);
                        _dbContext.SaveChanges();

                    }


                    _dbContext.Colleges.Remove(record);
                    _dbContext.SaveChanges();
                    ajaxResponse.Success = true;
                }
               
            }
            if (academicId > 0)
            {
                var academic = _dbContext.AcademicHonors.FirstOrDefault(c => c.AcademicHonorId == academicId);
                    _dbContext.AcademicHonors.Remove(academic);
                    _dbContext.SaveChanges();

                
            }
            if (scholarshipid > 0)
            {
                var scholarship = _dbContext.AcademicScholarships.FirstOrDefault(c => c.AcademicScholarshipId == scholarshipid);
                    _dbContext.AcademicScholarships.Remove(scholarship);
                    _dbContext.SaveChanges();

                
            }

            return Json(ajaxResponse);
        }


        public IActionResult GetAllDegrees()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetDegrees();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }
        public IActionResult GetAllMajor()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetMajors();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetAllStates()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetStates();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }
        public IActionResult GetAllMinors()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetMinors();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetAllCertificates()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetCertificates();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetAllMajorSpecilties()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetMajorSpecialties();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

    }
}
