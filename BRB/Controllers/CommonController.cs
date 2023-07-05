﻿using BusinessObjects.Models.DTOs;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace BRB.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IDropdownService _dropdownService;
        public CommonController(IDropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        public IActionResult GetCountryList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetCountries();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetStateList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetStates();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetLanguageAbilityList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetLanguageAbility();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult IsOptOut(int recordId , int sectionId,bool status)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            switch (sectionId)
            {
               
                case 30:
                    var overseasRecord = _dbContext.OverseasExperiences.FirstOrDefault(x => x.OverseasExperienceId == recordId);
                    if (overseasRecord != null)
                    {
                        overseasRecord.IsOptOut = status;
                        _dbContext.OverseasExperiences.Update(overseasRecord);
                        _dbContext.SaveChanges();
                        ajaxResponse.Data = overseasRecord;
                        ajaxResponse.Success = true;
                    }
                    break;
                case 40:
                    var miltaryRecord = _dbContext.MilitaryExperiences.FirstOrDefault(x => x.MilitaryExperienceId == recordId);
                    if (miltaryRecord != null)
                    {
                        miltaryRecord.IsOptOut = status;
                        _dbContext.MilitaryExperiences.Update(miltaryRecord);
                        _dbContext.SaveChanges();
                        ajaxResponse.Data = miltaryRecord;
                        ajaxResponse.Success = true;
                    }
                  
                    break;
                case 45:
                    var organization = _dbContext.OrgExperiences.FirstOrDefault(x => x.OrgExperienceId == recordId);
                    if (organization != null)
                    {
                        organization.IsOptOut = status;
                        _dbContext.OrgExperiences.Update(organization);
                        _dbContext.SaveChanges();
                        ajaxResponse.Data = organization;
                        ajaxResponse.Success = true;
                    }
                   
                    break;
                case 50:
                    var communityService = _dbContext.VolunteerExperiences.FirstOrDefault(x => x.VolunteerExperienceId == recordId);
                    communityService.IsOptOut = status;
                    _dbContext.VolunteerExperiences.Update(communityService);
                    _dbContext.SaveChanges();
                    ajaxResponse.Data = communityService;
                    ajaxResponse.Success = true;
                    break;
                case 55:
                    var Professional = _dbContext.Professionals.FirstOrDefault(x => x.ProfessionalId == recordId);
                    Professional.IsOptOut = status;
                    _dbContext.Professionals.Update(Professional);
                    _dbContext.SaveChanges();
                    ajaxResponse.Data = Professional;
                    ajaxResponse.Success = true;
                    break;
                case 65:
                    var language = _dbContext.LanguageSkills.FirstOrDefault(x => x.LanguageSkillId == recordId);
                    language.IsOptOut = status;
                    _dbContext.LanguageSkills.Update(language);
                    _dbContext.SaveChanges();
                    ajaxResponse.Data= language;
                    ajaxResponse.Success = true;
                    break;

            }
            return Json(ajaxResponse);
        }
    }
}
