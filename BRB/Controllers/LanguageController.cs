using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BRB.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly ILanguageService _languageService;
        private readonly IResumeService _resumeService;
        public LanguageController(ILanguageService languageService, IResumeService resumeService)
        {
            _languageService = languageService;
            _resumeService = resumeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetLanguageSkillsRecord()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;

            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _languageService.GetLanguageRecord(sessionData.ResumeId);
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult PostLanguageSkillData(LanguageViewModel languageViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Redirect = "/Resume/Home";
            ajaxResponse.Data = null;
            ajaxResponse.Success = true;
            var masterData = new LanguageSkill();
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            if (sessionData != null)
            {
                
                Resume resumeProfileData = new Resume();
                resumeProfileData.ResumeId = sessionData.ResumeId;
                resumeProfileData.UserId = sessionData.UserId;
                resumeProfileData.LastSectionVisitedId = languageViewModel.LastSectionVisitedId;
                resumeProfileData.LastModDate = DateTime.Today;
                resumeProfileData.CreatedDate = DateTime.Today;
                resumeProfileData.LastSectionCompletedId = languageViewModel.IsComplete == true ? languageViewModel.LastSectionVisitedId : 0;
                LanguageSkill languageSkill = new LanguageSkill();
                languageSkill.ResumeId = sessionData.ResumeId;
                languageSkill.LanguageSkillId = languageViewModel.LanguageSkillId;
                languageSkill.IsComplete = languageViewModel.IsComplete;
                languageSkill.CreatedDate = DateTime.Today;
                languageSkill.LastModDate = DateTime.Today;
                using (var transection = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (languageSkill.LanguageSkillId > 0)
                        {
                            _dbContext.LanguageSkills.Update(languageSkill);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            var record = _dbContext.LanguageSkills.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                            if (record != null)
                            {
                                _dbContext.LanguageSkills.Add(languageSkill);
                                _dbContext.SaveChanges();
                            }
                        }
                        if (languageViewModel.Languages.Count > 0)
                        {
                            foreach (var l in languageViewModel.Languages)
                            {
                                l.LanguageSkillId = languageSkill.LanguageSkillId;
                                l.CreatedDate = DateTime.Today;
                                l.LastModDate = DateTime.Today;
                                if (l.LanguageId > 0)
                                {
                                    _dbContext.Languages.Update(l);
                                   
                                }
                                else
                                {
                                    _dbContext.Languages.Add(l);
                                   
                                }
                            }
                        }
                        _dbContext.Resumes.Update(resumeProfileData);
                        _dbContext.SaveChanges();
                        transection.Commit();
                    }
                    catch (Exception)
                    {

                        transection.Rollback();
                    }
                }

            }
            return Json(ajaxResponse);
        }

        public IActionResult GetLanguageById(int languageId)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _languageService.GetLanguageData(languageId);
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        [HttpPost]

        public IActionResult DeleteRecord(int id)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            var record = _dbContext.Languages.FirstOrDefault(x => x.LanguageId == id);
            if (record != null)
            {
                _dbContext.Languages.Remove(record);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }

    }
}
