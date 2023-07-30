using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
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
            ajaxResponse.Redirect = "/resume/military";
            var overseasExperienceData = new OverseasExperience();
            Resume resume = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            resume.ResumeId = sessionData.ResumeId;
            resume.UserId = sessionData.UserId;
            resume.LastSectionVisitedId = overseasStudyViewModel.LastSectionVisitedId;
            resume.LastModDate = DateTime.Today;
            resume.CreatedDate = DateTime.Today;
            resume.GeneratedFileName = null;
            resume.LastSectionCompletedId = overseasStudyViewModel.IsComplete == true ? overseasStudyViewModel.LastSectionVisitedId : 0;
            OverseasExperience overseasExperience = new OverseasExperience();
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var data = _dbContext.OverseasExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (sessionData != null)
                    {
                       
                        if (data == null)
                        {
                           
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


                            data.IsOptOut = overseasStudyViewModel.IsOptOut;
                            data.ResumeId = sessionData.ResumeId;
                            data.IsComplete = overseasStudyViewModel.IsComplete;
                            data.CreatedDate = DateTime.Today;
                            data.LastModDate = DateTime.Today;
                            _dbContext.OverseasExperiences.Update(data);
                        }
                        if (overseasStudyViewModel.OverseasStudies.Count > 0)
                        {
                            foreach (var overseas in overseasStudyViewModel.OverseasStudies)
                            {
                                overseas.CreatedDate = DateTime.Today;
                                overseas.LastModDate = DateTime.Today;
                               
                                if (overseas.OverseasStudyId > 0)
                                {
                                    overseas.OverseasExperienceId = data.OverseasExperienceId;
                                    _dbContext.OverseasStudies.Update(overseas);
                                }
                                else
                                {
                                    overseas.OverseasExperienceId = data.OverseasExperienceId;
                                    _dbContext.OverseasStudies.Add(overseas);
                                }
                            }
                            _dbContext.SaveChanges();
                        }
                        _dbContext.Resumes.Update(resume);
                        _dbContext.SaveChanges();   
                        transection.Commit();
                       

                    }
                }
                catch (Exception)
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
            var data = _dbContext.OverseasStudies.Where(x => x.OverseasExperienceId == masterId).ToList();
            foreach (var item in data)
            {
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
