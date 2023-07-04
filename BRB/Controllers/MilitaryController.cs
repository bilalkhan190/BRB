using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BRB.Controllers
{
    public class MilitaryController : BaseController
    {
        private readonly IMilitaryService _militaryService;
        private readonly IResumeService _resumeService;
        private readonly IMapper _mapper;
        public MilitaryController(IMilitaryService militaryService, IResumeService resumeService, IMapper mapper)
        {
            _militaryService = militaryService;
            _resumeService = resumeService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult PostMilitaryData(MilitaryViewModel militaryViewModel) 
        
        { 
           AjaxResponse ajaxResponse = new AjaxResponse();
            MilitaryExperience militaryExperience = new MilitaryExperience();
            ajaxResponse.Data = null;
            ajaxResponse.Redirect = "/Resume/Organizations";
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            //updating resume
            ResumeViewModels resumeProfileData = new ResumeViewModels();
            resumeProfileData.UserId = sessionData.UserId;
            resumeProfileData.ResumeId = sessionData.ResumeId;
            resumeProfileData.LastModDate = DateTime.Today;
            resumeProfileData.LastSectionVisitedId = militaryViewModel.LastSectionVisitedId;
            resumeProfileData.LastSectionCompletedId = militaryViewModel.IsComplete == true ? militaryViewModel.LastSectionVisitedId : 0;
            _resumeService.UpdateResumeMaster(resumeProfileData);
              militaryViewModel.ResumeId = sessionData.ResumeId;
              militaryViewModel.CreatedDate = DateTime.Today;
              militaryViewModel.LastModDate = DateTime.Today;
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (militaryViewModel.MilitaryExperienceId == 0)
                    {
                       
                        militaryExperience.MilitaryExperienceId = militaryViewModel.MilitaryExperienceId;
                        militaryExperience.CountryId = militaryViewModel.CountryId;
                        militaryExperience.Rank = militaryViewModel.Rank;
                        militaryExperience.Branch = militaryViewModel.Branch;
                        militaryExperience.City = militaryViewModel.City;
                        militaryExperience.CreatedDate = DateTime.Today;
                        militaryExperience.LastModDate = DateTime.Today;
                        militaryExperience.ResumeId = militaryViewModel.ResumeId;
                        militaryExperience.EndedYear = militaryViewModel.EndedYear;
                        militaryExperience.EndedMonth = militaryViewModel.EndedMonth;
                        militaryExperience.EndedMonth = militaryViewModel.EndedMonth;
                        militaryExperience.StartedMonth = militaryViewModel.StartedMonth;
                        militaryExperience.IsComplete = militaryViewModel.IsComplete;
                        militaryExperience.IsOptOut = militaryViewModel.IsOptOut;
                        _dbContext.MilitaryExperiences.Add(militaryExperience);
                        _dbContext.SaveChanges();
                    }
                
                    else
                    {
                     
                        militaryExperience.MilitaryExperienceId = militaryViewModel.MilitaryExperienceId;
                        militaryExperience.CountryId = militaryViewModel.CountryId;
                        militaryExperience.Rank = militaryViewModel.Rank;
                        militaryExperience.Branch = militaryViewModel.Branch;
                        militaryExperience.City = militaryViewModel.City;
                        militaryExperience.CreatedDate = DateTime.Today;
                        militaryExperience.LastModDate = DateTime.Today;
                        militaryExperience.ResumeId = militaryViewModel.ResumeId;
                        militaryExperience.StartedYear = militaryViewModel.StartedYear;
                        militaryExperience.EndedYear = militaryViewModel.EndedYear;
                        militaryExperience.EndedMonth = militaryViewModel.EndedMonth;
                        militaryExperience.StartedMonth = militaryViewModel.StartedMonth;
                        militaryExperience.IsComplete = militaryViewModel.IsComplete;
                        militaryExperience.IsOptOut = militaryViewModel.IsOptOut;
                        _dbContext.MilitaryExperiences.Update(militaryExperience);
                        _dbContext.SaveChanges();
                    }
                    if (militaryViewModel.MilitaryPositions.Count > 0)
                    {
                        foreach (var mp in militaryViewModel.MilitaryPositions)
                        {
                            mp.MilitaryExperienceId = militaryExperience.MilitaryExperienceId;
                            mp.CreatedDate = DateTime.Today;
                            mp.LastModDate = DateTime.Today;
                            if (mp.MilitaryPositionId > 0)
                            {
                                _dbContext.MilitaryPositions.Update(mp);
                                _dbContext.SaveChanges();
                            }
                            else
                            {
                                _dbContext.MilitaryPositions.Add(mp);
                                _dbContext.SaveChanges();
                                //_militaryService.AddMilitaryPosition(mp,_dbContext);
                            }

                        }
                    }
                    transection.Commit();
                }
                catch (Exception)
                {
                    transection.Rollback();
                    throw;
                }
            }
            
           

            ajaxResponse.Data = militaryViewModel;
            
            return Json(ajaxResponse);  
        }

        public IActionResult GetMilataryExperience()
        {
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _militaryService.GetMilitaryExperienceByResumeId(sessionData.ResumeId);
            if (record != null)
            {
                ajaxResponse.Data = record; 
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetPositionById(int id)
        {
            AjaxResponse response = new AjaxResponse();
            var record = _militaryService.GetMilitaryPositionByoId(id);
            if (record != null)
            {
                response.Data = record;
            }
            return Json(response);
        }

        [HttpPost]

        public IActionResult Delete(int id)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            var record = _dbContext.MilitaryPositions.FirstOrDefault(x => x.MilitaryPositionId == id);
            if (record != null)
            {
                _dbContext.MilitaryPositions.Remove(record);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }
    }
}
