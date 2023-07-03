using BusinessObjects.Models.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BusinessObjects.Services.interfaces;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult GetData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = true;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            List<VolunteerViewObject> ListOfObjs = new List<VolunteerViewObject>();
            var record = _dbContext.VolunteerOrgs.Where(x => x.VolunteerExperienceId == ids.volunteerId).ToList();
            foreach (var o in record)
            {
                VolunteerViewObject viewObject = new VolunteerViewObject();
                viewObject.VolunteerOrg = o;
                viewObject.VolunteerPositions = GetOrgPositions(o.VolunteerOrgId);
                ListOfObjs.Add(viewObject);
            }
            ajaxResponse.Data = ListOfObjs;
            return Json(ajaxResponse);


        }
        private List<VolunteerPosition> GetOrgPositions(int orgId)
        {
            return _dbContext.VolunteerPositions.Where(x => x.VolunteerOrgId == orgId).ToList();
        }


        public IActionResult PostCommunityService(CommunityViewModel communityViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = true;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            Resume resumeProfileData = new Resume();
            resumeProfileData.ResumeId = sessionData.ResumeId;
            resumeProfileData.UserId = sessionData.UserId;
            resumeProfileData.LastSectionVisitedId = communityViewModel.LastSectionVisitedId;
            resumeProfileData.LastModDate = DateTime.Today;
            resumeProfileData.CreatedDate = DateTime.Today;
            resumeProfileData.LastSectionCompletedId = communityViewModel.IsComplete == true ? communityViewModel.LastSectionVisitedId : 0;
            VolunteerExperience volunteerExperience = new VolunteerExperience();

            var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            var master = _dbContext.VolunteerExperiences.FirstOrDefault(x => x.VolunteerExperienceId == ids.volunteerId);
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (master != null)
                    {


                        volunteerExperience.VolunteerExperienceId = master.VolunteerExperienceId;
                        volunteerExperience.ResumeId = sessionData.ResumeId;
                        volunteerExperience.CreatedDate = DateTime.Today;
                        volunteerExperience.IsOptOut = false;
                        volunteerExperience.IsComplete = communityViewModel.IsComplete;
                        _dbContext.VolunteerExperiences.Entry(volunteerExperience).State = EntityState.Modified;
                        _dbContext.SaveChanges();

                    }
                    else
                    {
                        volunteerExperience.ResumeId = sessionData.ResumeId;
                        volunteerExperience.CreatedDate = DateTime.Today;
                        volunteerExperience.IsOptOut = false;
                        volunteerExperience.IsComplete = communityViewModel.IsComplete;
                        _dbContext.VolunteerExperiences.Add(volunteerExperience);
                        _dbContext.SaveChanges();
                    }

                    if (communityViewModel.VolunteerOrgs.Count > 0)
                    {
                        foreach (var org in communityViewModel.VolunteerOrgs)
                        {

                            org.VolunteerExperienceId = volunteerExperience.VolunteerExperienceId;
                            org.CreatedDate = DateTime.Today;
                            org.LastModDate = DateTime.Today;
                            if (org.VolunteerOrgId > 0)
                            {
                                _dbContext.VolunteerOrgs.Entry(org).State = EntityState.Modified;
                            }
                            else
                            {
                                _dbContext.VolunteerOrgs.Add(org);

                            }

                            if (communityViewModel.VolunteerPositions.Count > 0)
                            {
                                foreach (var position in communityViewModel.VolunteerPositions)
                                {
                                    position.VolunteerOrgId = org.VolunteerOrgId;
                                    position.CreatedDate = DateTime.Today;
                                    position.LastModDate = DateTime.Today;
                                    if (position.VolunteerPositionId > 0)
                                    {
                                        _dbContext.VolunteerPositions.Entry(position).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        _dbContext.VolunteerOrgs.Add(org);

                                    }
                                }
                            }
                            _dbContext.Resumes.Update(resumeProfileData);  
                        }
                        _dbContext.SaveChanges();
                        transection.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw;
                }
            }
           
        
            return Json(ajaxResponse);
        }
    }
}
