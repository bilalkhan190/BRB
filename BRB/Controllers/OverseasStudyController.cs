using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using System.Text.Json;

namespace BRB.Controllers
{
    public class OverseasStudyController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IOverseasStudyService _overseasStudyService;
        private readonly IMapper _mapper;

        public OverseasStudyController(IOverseasStudyService overseasStudyService,IResumeService resumeService,IMapper mapper)
        {
            _overseasStudyService = overseasStudyService;
            _resumeService = resumeService; 
            _mapper = mapper;
        }
        public IActionResult PostData(OverseasStudyViewModel overseasStudyViewModel)
        
         {
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            //var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            ajaxResponse.Message = string.Empty;
            ajaxResponse.Redirect = "/resume/WorkExperience";
            var overseasExperienceData = new OverseasExperience();
            Resume resume = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            resume.ResumeId = sessionData.ResumeId;
            resume.UserId = sessionData.UserId;
            resume.LastSectionVisitedId = overseasStudyViewModel.LastSectionVisitedId;
            resume.LastModDate = DateTime.Today;
            resume.CreatedDate = DateTime.Today;
            resume.GeneratedFileName = null;
            resume.LastSectionCompletedId = overseasStudyViewModel.IsComplete == true ? overseasStudyViewModel.LastSectionVisitedId : 0;
            
            OverseasExperience overseasExperience = null;
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    overseasExperience = _dbContext.OverseasExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (sessionData != null)
                    {
                       
                        if (overseasExperience == null)
                        {

                            overseasExperience = new OverseasExperience();
                            overseasExperience.IsOptOut = overseasStudyViewModel.IsOptOut;
                            overseasExperience.ResumeId = sessionData.ResumeId;
                            overseasExperience.IsComplete = overseasStudyViewModel.IsComplete;
                            overseasExperience.CreatedDate = DateTime.Today;
                            overseasExperience.LastModDate = DateTime.Today;
                           _dbContext.OverseasExperiences.Add(overseasExperience);
                            _dbContext.SaveChanges();
                        }
                        else
                        {


                            overseasExperience.IsOptOut = overseasStudyViewModel.IsOptOut;
                            overseasExperience.ResumeId = sessionData.ResumeId;
                            overseasExperience.IsComplete = overseasStudyViewModel.IsComplete;
                            overseasExperience.CreatedDate = DateTime.Today;
                            overseasExperience.LastModDate = DateTime.Today;
                            _dbContext.OverseasExperiences.Update(overseasExperience);
                        }
                        if (overseasStudyViewModel.OverseasStudies.Count > 0)
                        {
                            foreach (var overseas in overseasStudyViewModel.OverseasStudies)
                            {

                               
                               
                                if (overseas.OverseasStudyId > 0)
                                {
                                    var record = _dbContext.OverseasStudies.FirstOrDefault(x => x.OverseasStudyId == overseas.OverseasStudyId);
                                    if (record != null)
                                    {
                                        overseas.LastModDate = DateTime.Today;
                                        overseas.CreatedDate = record.CreatedDate;
                                        overseas.OverseasExperienceId = overseasExperience.OverseasExperienceId;
                                        overseas.StartedDate = new DateTime(DateTime.ParseExact(overseas.StartedYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(overseas.StartedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1);
                                        overseas.EndedDate = !(string.IsNullOrEmpty(overseas.EndedYear) && string.IsNullOrEmpty(overseas.EndedMonth)) ? new DateTime(DateTime.ParseExact(overseas.EndedYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(overseas.EndedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1) : null;
                                        var checkTracking = _dbContext.Set<OverseasStudy>().Local.FirstOrDefault(x => x.OverseasStudyId == record.OverseasStudyId);
                                        if (checkTracking != null) _dbContext.Entry<OverseasStudy>(record).State = EntityState.Detached;
                                        _dbContext.OverseasStudies.Update(overseas);
                                        _dbContext.SaveChanges();
                                    }
                                }
                                else
                                {
                                    overseas.LastModDate = DateTime.Today;
                                    overseas.CreatedDate = DateTime.Today;
                                    overseas.OverseasExperienceId = overseasExperience.OverseasExperienceId;
                                    overseas.StartedDate = new DateTime(DateTime.ParseExact(overseas.StartedYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(overseas.StartedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1);
                                    overseas.EndedDate = !(string.IsNullOrEmpty(overseas.EndedYear) && string.IsNullOrEmpty(overseas.EndedMonth)) ? new DateTime(DateTime.ParseExact(overseas.EndedYear, "yyyy", CultureInfo.CurrentCulture).Year, DateTime.ParseExact(overseas.EndedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1):null;
                                    _dbContext.OverseasStudies.Add(overseas);
                                    _dbContext.SaveChanges();
                                }
                            }
                            _dbContext.SaveChanges();
                        }
                        _dbContext.Resumes.Update(resume);
                        _dbContext.SaveChanges();   
                        transection.Commit();
                       

                    }
                }
                catch (Exception ex)
                {

                    transection.Rollback();
                }
            }
           
            return Json(ajaxResponse);  
        }

        public IActionResult GetMasterData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            OverseasExperience overseasExperience = new OverseasExperience();
            ajaxResponse.Data = null;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _dbContext.OverseasExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (record != null)
            {
                record.OverseasStudies = GetOverseasStudies(record.OverseasExperienceId);
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        private List<OverseasStudy> GetOverseasStudies(int masterId)
        {
            List<OverseasStudy> list = new List<OverseasStudy>();
            var data = _dbContext.OverseasStudies.Where(x => x.OverseasExperienceId == masterId).OrderByDescending(x =>x.CreatedDate).ToList();
           
            foreach (var item in data)
            {
                item.CountryName = _dbContext.CountryLists.FirstOrDefault(x => x.CountryId == item.CountryId)?.CountryName;
                    
                if (!string.IsNullOrEmpty(item.StartedDate.ToString()))
                {
                    var startDate = Convert.ToDateTime(item.StartedDate);
                           item.StartedMonth = startDate.ToString("MMMM");
                    item.StartedYear = startDate.ToString("yyyy");
                }
                if (!string.IsNullOrEmpty(item.EndedDate.ToString()))
                {
                    var endDate = Convert.ToDateTime(item.EndedDate);
                    item.EndedMonth = endDate.ToString("MMMM");
                    item.EndedYear = endDate.ToString("yyyy");
                }  
                list.Add(item);
            }
            return list;
        }
        //public IActionResult GetDataById()
        //{
        //    AjaxResponse ajaxResponse = new AjaxResponse();
        //    ajaxResponse.Data = null;
        //    var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
        //    var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
        //    var record = _dbContext.OverseasStudies.Where(x => x.OverseasExperienceId == sessionData.ResumeId).ToList();
        //    if (record != null)
        //    {
        //        ajaxResponse.Data = record;
        //    }
        //    return Json(ajaxResponse);
        //}

        public IActionResult GetLivingSituationData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _overseasStudyService.GetLivingSituationList();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record.OrderByDescending(x => x.LivingSituationId);
            }
            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            var record = _dbContext.OverseasStudies.FirstOrDefault(x => x.OverseasStudyId == id);
            if (record != null)
            {
                _dbContext.OverseasStudies.Remove(record);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
           return Json(ajaxResponse);
        }
    }
}
