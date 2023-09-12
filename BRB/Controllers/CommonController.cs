using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

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
        public IActionResult IsOptOut(int recordId, int sectionId, bool status)
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            switch (sectionId)
            {

                case 30:
                    var record = _dbContext.OverseasExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (recordId == 0 || record == null)
                    {
                        OverseasExperience overseasExperience = new OverseasExperience();
                        overseasExperience.IsOptOut = status;
                        overseasExperience.ResumeId = sessionData.ResumeId;
                        overseasExperience.CreatedDate = DateTime.Today;
                        overseasExperience.LastModDate = DateTime.Today;
                        overseasExperience.IsComplete = false;
                        _dbContext.OverseasExperiences.Add(overseasExperience);
                        _dbContext.SaveChanges();
                        recordId = overseasExperience.OverseasExperienceId;
                    }

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
                    var militaryExperience = _dbContext.MilitaryExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (recordId == 0 || militaryExperience == null)
                    {
                        MilitaryExperience military = new MilitaryExperience();
                        military.IsOptOut = status;
                        military.ResumeId = sessionData.ResumeId;
                        military.CreatedDate = DateTime.Today;
                        military.LastModDate = DateTime.Today;
                        military.IsComplete = false;
                        _dbContext.MilitaryExperiences.Add(military);
                        _dbContext.SaveChanges();
                        recordId = military.MilitaryExperienceId;
                    }
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
                    var orgExperience = _dbContext.OrgExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (recordId == 0 || orgExperience == null)
                    {
                        OrgExperience org = new OrgExperience();
                        org.IsOptOut = status;
                        org.ResumeId = sessionData.ResumeId;
                        org.CreatedDate = DateTime.Today;
                        org.LastModDate = DateTime.Today;
                        org.IsComplete = false;
                        _dbContext.OrgExperiences.Add(org);
                        _dbContext.SaveChanges();
                        recordId = org.OrgExperienceId;
                    }
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
                    var volunteer = _dbContext.VolunteerExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (recordId == 0 || volunteer == null)
                    {
                        VolunteerExperience vol = new VolunteerExperience();
                        vol.IsOptOut = status;
                        vol.ResumeId = sessionData.ResumeId;
                        vol.CreatedDate = DateTime.Today;
                        vol.LastModDate = DateTime.Today;
                        vol.IsComplete = false;
                        _dbContext.VolunteerExperiences.Add(vol);
                        _dbContext.SaveChanges();
                        recordId = vol.VolunteerExperienceId;
                    }
                    var communityService = _dbContext.VolunteerExperiences.FirstOrDefault(x => x.VolunteerExperienceId == recordId);
                    communityService.IsOptOut = status;
                    _dbContext.VolunteerExperiences.Update(communityService);
                    _dbContext.SaveChanges();
                    ajaxResponse.Data = communityService;
                    ajaxResponse.Success = true;
                    break;
                case 55:
                    var professional = _dbContext.Professionals.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (recordId == 0 || professional == null)
                    {
                        Professional pro = new Professional();
                        pro.IsOptOut = status;
                        pro.ResumeId = sessionData.ResumeId;
                        pro.CreatedDate = DateTime.Today;
                        pro.LastModDate = DateTime.Today;
                        pro.IsComplete = false;
                        _dbContext.Professionals.Add(pro);
                        _dbContext.SaveChanges();
                        recordId = pro.ProfessionalId;
                    }
                    var Professional = _dbContext.Professionals.FirstOrDefault(x => x.ProfessionalId == recordId);
                    Professional.IsOptOut = status;
                    _dbContext.Professionals.Update(Professional);
                    _dbContext.SaveChanges();
                    ajaxResponse.Data = Professional;
                    ajaxResponse.Success = true;
                    break;
                case 65:
                    var languageObj = _dbContext.LanguageSkills.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (recordId == 0 || languageObj == null)
                    {
                        LanguageSkill ls = new LanguageSkill();
                        ls.IsOptOut = status;
                        ls.ResumeId = sessionData.ResumeId;
                        ls.CreatedDate = DateTime.Today;
                        ls.LastModDate = DateTime.Today;
                        ls.IsComplete = false;
                        _dbContext.LanguageSkills.Add(ls);
                        _dbContext.SaveChanges();
                        recordId = ls.LanguageSkillId;
                    }
                    var language = _dbContext.LanguageSkills.FirstOrDefault(x => x.LanguageSkillId == recordId);
                    language.IsOptOut = status;
                    _dbContext.LanguageSkills.Update(language);
                    _dbContext.SaveChanges();
                    ajaxResponse.Data = language;
                    ajaxResponse.Success = true;
                    break;

            }
            return Json(ajaxResponse);
        }


        public IActionResult GetIsOptOut()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            //var record = _dropdownService.GetLanguageAbility();
            //is opr out

            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var dtIsOptOut = SqlHelper.GetDataTable("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetIsOptOut", CommandType.StoredProcedure, new SqlParameter("ResumeId", sessionData.ResumeId));
            if (dtIsOptOut.Rows.Count > 0)
            {
                string IsOptOutJson = JsonConvert.SerializeObject(dtIsOptOut);
                ajaxResponse.Data = IsOptOutJson;
            }
            else
            {
                ajaxResponse.Data = null;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetStateName(string stateAbbr)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var stateName = _dbContext.StateLists.FirstOrDefault(x => x.StateAbbr == stateAbbr).StateName;
            if (!string.IsNullOrEmpty(stateName))
            {
                ajaxResponse.Data = stateName;
                ajaxResponse.Success = true;
            }

            
            return Json(ajaxResponse);
        }

    }
}
