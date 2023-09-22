using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BRB.Controllers
{
    public class ObjectiveSummaryController : BaseController
    {
        private readonly IDropdownService _dropdownService;
        private readonly IObjectiveService _objectiveSummaryService;
        private readonly IResumeService _resumeService;
        private readonly IMapper _mapper;
        public ObjectiveSummaryController(IDropdownService dropdownService, IObjectiveService objectiveSummaryService, IResumeService resumeService, IMapper mapper)
        {
            _dropdownService = dropdownService;
            _objectiveSummaryService = objectiveSummaryService;
            _resumeService = resumeService;
            _mapper = mapper;
        }

        public IActionResult GetAllData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var objectives = _dropdownService.GetObjectives();
            var record = _dbContext.ObjectiveSummaries.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetExperienceYears()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetYearsOfExperience();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetPositionType()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetPositionTypes();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetChangeType()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetChangeType();
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetObjectives()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _dropdownService.GetObjectives();
           
            var checkboxes = _dbContext.ObjectiveSummaries.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (checkboxes != null)
            {
                record.Where(x => x.ObjectiveId == checkboxes.Objective1Id || x.ObjectiveId == checkboxes.Objective2Id || x.ObjectiveId == checkboxes.Objective3Id).ToList()
                     
              .ForEach(record => record.Checked = true);
            }
           
            if (record != null)
            {
               
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult PostData(ObjectiveSummeryViewModel objectiveSummeryViewModel)
        {
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Message = "";
            ajaxResponse.Success = false;
            ajaxResponse.Redirect = "/Resume/Education";
            objectiveSummeryViewModel.ResumeId = sessionData.ResumeId;
            objectiveSummeryViewModel.CreatedDate = DateTime.Today;
            objectiveSummeryViewModel.LastModDate = DateTime.Today;
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var model = _mapper.Map<ObjectiveSummary>(objectiveSummeryViewModel);
                    if (objectiveSummeryViewModel.ObjectiveSummaryId > 0)
                    {
                        
                        _dbContext.ObjectiveSummaries.Update(model);
                    }
                    else
                    {
                        _dbContext.ObjectiveSummaries.Add(model);
                        
                    }
                    Resume resumeProfileData = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    resumeProfileData.ResumeId = sessionData.ResumeId;
                    resumeProfileData.UserId = sessionData.UserId;
                    resumeProfileData.LastSectionVisitedId = objectiveSummeryViewModel.LastSectionVisitedId;
                    resumeProfileData.LastModDate = DateTime.Today;
                    resumeProfileData.LastSectionCompletedId = objectiveSummeryViewModel.IsComplete == true ? objectiveSummeryViewModel.LastSectionVisitedId : 0;

                    resumeProfileData.LastModDate = DateTime.Today;
                    resumeProfileData.CreatedDate = DateTime.Today;
                    _dbContext.Resumes.Update(resumeProfileData);
                    _dbContext.SaveChanges();
                    transection.Commit();
                }
                catch (Exception)
                {

                    transection.Rollback();
                }
            }
           
            return Json(ajaxResponse);
        }
    }
}
