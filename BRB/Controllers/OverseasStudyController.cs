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
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            ajaxResponse.Message = string.Empty;
            ajaxResponse.Redirect = "/resume/military";
            var overseasExperienceData = new OverseasExperience();
            Resume resume = new Resume();
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (sessionData != null)
                    {
                       
                        if (!_overseasStudyService.IsRecordExist(sessionData.ResumeId))
                        {
                            OverseasExperience overseasExperience = new OverseasExperience();
                            overseasExperience.IsOptOut = false;
                            overseasExperience.ResumeId = sessionData.ResumeId;
                            overseasExperience.IsComplete = false;
                            overseasExperience.CreatedDate = DateTime.Today;
                            overseasExperience.LastModDate = DateTime.Today;
                           _dbContext.OverseasExperiences.Add(overseasExperience);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                           
                            OverseasExperience overseasExperience = new OverseasExperience();
                            overseasExperience.IsOptOut = false;
                            overseasExperience.ResumeId = sessionData.ResumeId;
                            overseasExperience.IsComplete = false;
                            overseasExperience.CreatedDate = DateTime.Today;
                            overseasExperience.LastModDate = DateTime.Today;
                            overseasExperience.OverseasExperienceId = ids.overseasExpId;
                            _dbContext.OverseasExperiences.Update(overseasExperience);

                        }
                        if (overseasStudyViewModel.OverseasStudies.Count > 0)
                        {
                            foreach (var overseas in overseasStudyViewModel.OverseasStudies)
                            {
                                overseas.CreatedDate = DateTime.Today;
                                overseas.LastModDate = DateTime.Today;
                                overseas.OverseasExperienceId = ids.overseasExpId;
                                if (overseas.OverseasStudyId > 0)
                                {
                                    _dbContext.OverseasStudies.Update(overseas);
                                }
                                else
                                {
                                    _dbContext.OverseasStudies.Add(overseas);
                                }
                            }
                        }
                     
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

        public IActionResult GetDataById()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
           var ids  = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
           var record = _dbContext.OverseasStudies.Where(x => x.OverseasExperienceId == ids.overseasExpId).ToList();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);  
        }

        public IActionResult GetLivingSituationData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _overseasStudyService.GetLivingSituationList();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        } 
    }
}
