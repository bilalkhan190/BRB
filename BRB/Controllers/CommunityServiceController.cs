using BusinessObjects.Models.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BusinessObjects.Services.interfaces;

namespace BRB.Controllers
{
    public class CommunityServiceController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly ICommunityService _communityService;
        public CommunityServiceController(ICommunityService communityService,IResumeService resumeService)
        {
            _communityService = communityService;
            _resumeService = resumeService;
        }
      

        public IActionResult PostCommunityService(CommunityViewModel communityViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = true;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            ResumeViewModels resumeProfileData = new ResumeViewModels();
            resumeProfileData.ResumeId = sessionData.ResumeId;
            resumeProfileData.UserId = sessionData.UserId;
            resumeProfileData.LastSectionVisitedId = communityViewModel.LastSectionVisitedId;
            resumeProfileData.LastModDate = DateTime.Today;
            resumeProfileData.LastSectionCompletedId = communityViewModel.IsComplete == true ? communityViewModel.LastSectionVisitedId : 0;
            _resumeService.UpdateResumeMaster(resumeProfileData);
            VolunteerExperience volunteerExperience = new VolunteerExperience();
            volunteerExperience.ResumeId = sessionData.ResumeId;
            volunteerExperience.CreatedDate = DateTime.Today;
            volunteerExperience.IsOptOut = false;
            volunteerExperience.IsComplete = communityViewModel.IsComplete;

            var volunteerExpereinceData = _communityService.AddVolunteerExperience(volunteerExperience);
            if (communityViewModel.VolunteerOrgs.Count > 0)
            {
                foreach (var org in communityViewModel.VolunteerOrgs)
                {
                    org.VolunteerExperienceId = volunteerExpereinceData.VolunteerExperienceId;
                    var  volOrgData =  _communityService.AddVolunteerOrganization(org);
                    if (communityViewModel.VolunteerPositions.Count > 0)
                    {
                        foreach (var position in communityViewModel.VolunteerPositions)
                        {
                            position.VolunteerOrgId = volOrgData.VolunteerOrgId;
                            _communityService.AddVolunteerOrganizationPosition(position);
                        }
                    }
                }
            }
        
            return Json(ajaxResponse);
        }
    }
}
