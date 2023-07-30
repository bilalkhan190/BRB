using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BRB.Controllers
{
    public class WorkExperienceController : BaseController
    {
        private readonly IDropdownService _dropdownService;
        private readonly IWorkExperienceService _workExperienceService;
        public WorkExperienceController(IDropdownService dropdownService, IWorkExperienceService workExperienceService)
        {
            _dropdownService = dropdownService;
            _workExperienceService = workExperienceService;
        }
        public IActionResult GetJobResponsibility()
        {
            ViewBag.Responsibilities = _dropdownService.GetJobCategoryList();
            return PartialView("_WorkResponsibility");
        }
        public IActionResult GetResponsibilityFAQ(int jobCategoryId)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            ResponsibilityViewModel responsibilityViewModel = new ResponsibilityViewModel();
            var data = _workExperienceService.GetResponsibilityOptions(jobCategoryId);
            if (data != null)
            {
                responsibilityViewModel.ResponsibilityOptions = data;
            }
            var Questions = _workExperienceService.GetResponsibilityQuestions(jobCategoryId);
            if (data != null)
            {
                responsibilityViewModel.ResponsibilityQuestions = Questions;
            }
            ajaxResponse.Data = responsibilityViewModel;

            return Json(ajaxResponse);
        }

        
     
        public async Task<IActionResult> AddCompany(WorkCompany company)
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var contactInfo = new ContactInfo();
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = string.Empty;
            if (company != null)
            {
                var master = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                if (company.CompanyId == 0)
                {
                    company.WorkExperienceId = master.WorkExperienceId;
                    _dbContext.WorkCompanies.Add(company);
                }
                else
                {
                    _dbContext.WorkCompanies.Update(company);
                }
                _dbContext.SaveChanges();
                var companyList = _dbContext.WorkCompanies.Where(x => x.WorkExperienceId == master.WorkExperienceId).ToList();
                var html = await ApplicationHelper.RenderViewAsync(this, "_Companies", companyList, true);
                ajaxResponse.Data = html;
                ajaxResponse.Success = true;

            }             
            return Json(ajaxResponse);
        }




        public async Task<IActionResult> deleteCompany(int companyId)
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var contactInfo = new ContactInfo();
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = string.Empty;
            var master = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (companyId > 0)
            {

            }
            var companyList = _dbContext.WorkCompanies.Where(x => x.WorkExperienceId == master.WorkExperienceId).ToList();
            var html = await ApplicationHelper.RenderViewAsync(this, "_Companies", companyList, true);
            ajaxResponse.Data = html;
            ajaxResponse.Success = true;

            
            return Json(ajaxResponse);
        }



        public async Task<IActionResult> AddPosition(WorkPosition model)
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var contactInfo = new ContactInfo();
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = string.Empty;
            try
            {
                if (model != null)
                {
                    var master = _dbContext.WorkExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (model.PositionId == 0)
                    {
                        _dbContext.WorkPositions.Add(model);
                    }
                    else
                    {
                        _dbContext.WorkPositions.Update(model);
                    }
                    _dbContext.SaveChanges();

                    var responseRecords = _dbContext.ResponsibilityOptionsResponses.Where(x => x.PositionId == model.PositionId).ToList();
                    if (responseRecords.Count > 0)
                        _dbContext.ResponsibilityOptionsResponses.RemoveRange(responseRecords);
                    foreach (var options in model.responsibilityOptions)
                    {
                        options.PositionId = model.PositionId;
                        _dbContext.ResponsibilityOptionsResponses.Add(options);
                    }
                    _dbContext.SaveChanges();


                    var questionRecords = _dbContext.WorkRespQuestions.Where(x => x.PositionId == model.PositionId).ToList();
                    if (questionRecords.Count > 0)
                        _dbContext.WorkRespQuestions.RemoveRange(questionRecords);
                    foreach (var question in model.workRespQuestions)
                    {
                        question.PositionId = model.PositionId;
                        _dbContext.WorkRespQuestions.Add(question);
                    }
                    _dbContext.SaveChanges();

                    var positionList = _dbContext.WorkPositions.Where(x => x.CompanyId == model.CompanyId).ToList();
                    positionList.ForEach(f => { 
                        f.workRespQuestions = _dbContext.WorkRespQuestions.Where(x => x.PositionId == f.PositionId).ToList();
                        f.responsibilityOptions = _dbContext.ResponsibilityOptionsResponses.Where(x => x.PositionId == f.PositionId).ToList();
                        f.JobAwards = _dbContext.JobAwards.Where(x => x.CompanyJobId == f.PositionId).ToList();
                    });
                    var html = await ApplicationHelper.RenderViewAsync(this, "_positions", positionList, true);
                    ajaxResponse.Data = html;
                    ajaxResponse.FieldName = "_" + model.CompanyId;
                    ajaxResponse.Success = true;


                }
            }
            catch (Exception ex)
            {
                ajaxResponse.Message = ex.Message;
            }
          
            return Json(ajaxResponse);
        }

        public async Task<IActionResult> AddAward(JobAward model)
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = string.Empty;
            try
            {
                if (model != null)
                {
                    if (model.JobAwardId > 0)
                    {
                        var record = _dbContext.JobAwards.FirstOrDefault(x => x.JobAwardId == model.JobAwardId);
                        record.AwardDesc = model.AwardDesc;
                        record.AwardedMonth = model.AwardedMonth;
                        record.AwardedYear = model.AwardedYear;
                        record.LastModDate = DateTime.Today;
                    }
                    else
                    {
                        model.CreatedDate = DateTime.Today;
                        _dbContext.JobAwards.Add(model);
                    }
                   
                      
                    
                    _dbContext.SaveChanges();
                    var awardList = _dbContext.JobAwards.Where(x => x.CompanyJobId == model.CompanyJobId).ToList();
                   
                    var html = await ApplicationHelper.RenderViewAsync(this, "_awards", awardList, true);
                    ajaxResponse.Data = html;
                    ajaxResponse.FieldName = "_" + model.CompanyJobId;
                    ajaxResponse.Success = true;


                }
            }
            catch (Exception ex)
            {
                ajaxResponse.Message = ex.Message;
            }

            return Json(ajaxResponse);
        }
        public IActionResult GetResponsibilities()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var data = _dropdownService.GetJobCategoryList();
            if (data != null)
            {
                ajaxResponse.Data = data;
            }
            return Json(ajaxResponse);
        }

        public IActionResult UpdateExperienceMaster(bool isComplete)
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            ajaxResponse.Success = false;
            var data = _dbContext.WorkExperiences.FirstOrDefault(x =>x.ResumeId == sessionData.ResumeId);
            if (data != null)
            {
                data.IsComplete = isComplete;
                data.LastModDate = DateTime.Today;    
                _dbContext.SaveChanges();
                //resume master bhi update krwana h with last visited id
                ajaxResponse.Data = data;
                ajaxResponse.Redirect = "/Resume/Military";
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }
    }
}
