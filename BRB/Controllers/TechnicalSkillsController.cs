using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BRB.Controllers
{
    public class TechnicalSkillsController : BaseController
    {
        private readonly ITechnicalSkillService _technicalSkillService;
        private readonly IResumeService _resumeService;
        private readonly IMapper _mapper;
        public TechnicalSkillsController(ITechnicalSkillService technicalSkillService, IResumeService resumeService, IMapper mapper)
        {
            _technicalSkillService = technicalSkillService;
            _resumeService = resumeService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var record = _dbContext.TechnicalSkills.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult PostData(TechnicalSkillViewModel technicalSkill)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            ajaxResponse.Success = true;
            ajaxResponse.Redirect = "/Resume/LanguagesSkills";
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            technicalSkill.CreatedDate = DateTime.Today;
            technicalSkill.LastModDate = DateTime.Today;
            technicalSkill.ResumeId = sessionData.ResumeId;
            var model = _mapper.Map<TechnicalSkill>(technicalSkill);
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (technicalSkill.TechnicalSkillId > 0)
                    {
                       
                        _dbContext.TechnicalSkills.Update(model);
                    }
                    else
                    {
                        _dbContext.TechnicalSkills.Add(model);
                    }
                    Resume resumeProfileData = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    resumeProfileData.ResumeId = sessionData.ResumeId;
                    resumeProfileData.UserId = sessionData.UserId;
                    resumeProfileData.LastSectionVisitedId = technicalSkill.LastSectionVisitedId;
                    resumeProfileData.LastModDate = DateTime.Today;
                    resumeProfileData.CreatedDate = DateTime.Today;
                    resumeProfileData.LastSectionCompletedId = technicalSkill.IsComplete == true ? technicalSkill.LastSectionVisitedId : 0;
                    resumeProfileData.GeneratedFileName = null;
                    _dbContext.Resumes.Update(resumeProfileData);
                    _dbContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {

                   trans.Rollback();
                }
            }
          
           
           

            return Json(ajaxResponse);
        }
    }
}
