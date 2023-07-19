using BusinessObjects.Models.DTOs;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BusinessObjects.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Microsoft.AspNetCore.Localization;

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
            List<VolunteerViewObject> ListOfObjs = new List<VolunteerViewObject>();
            VolunteerViewObject volunteerViewObject = new VolunteerViewObject();
            var masterRecord = _dbContext.VolunteerExperiences.FirstOrDefault(x =>x.ResumeId == sessionData.ResumeId);
            if (masterRecord != null)
            {
                
                var record = _dbContext.VolunteerOrgs.Where(x => x.VolunteerExperienceId == masterRecord.VolunteerExperienceId).ToList();


                if (record.Count > 0)
                {
                    foreach (var o in record)
                    {
                        VolunteerViewObject viewObject = new VolunteerViewObject();
                        viewObject.VolunteerExperience = masterRecord;
                        viewObject.VolunteerOrg = o;
                        viewObject.VolunteerPositions = GetOrgPositions(o.VolunteerOrgId);
                        ListOfObjs.Add(viewObject);
                    }
                    ajaxResponse.Data = ListOfObjs;
                }

            }
            else
            {
                ajaxResponse.Data = null;
            }


            return Json(ajaxResponse);


        }

        public IActionResult GetMasterData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            var record = _dbContext.VolunteerExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (record != null)
            {
                ajaxResponse.Data = record;
            }
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

            //var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
           
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var record = _dbContext.VolunteerExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                    if (record != null)
                    {
                        record.LastModDate = DateTime.Today;
                        record.IsOptOut = communityViewModel.IsOptOut;
                        record.IsComplete = communityViewModel.IsComplete;
                        _dbContext.SaveChanges();
                        volunteerExperience.VolunteerExperienceId = record.VolunteerExperienceId;
                    }
                    else
                    {
                        
                        volunteerExperience.ResumeId = sessionData.ResumeId;
                        volunteerExperience.CreatedDate = DateTime.Today;
                        volunteerExperience.IsOptOut = communityViewModel.IsOptOut;
                        volunteerExperience.IsComplete = communityViewModel.IsComplete;
                        _dbContext.VolunteerExperiences.Add(volunteerExperience);
                        _dbContext.SaveChanges();
                    }
                    if (communityViewModel.VolunteerOrgs.Count > 0)
                    {
                        foreach (var org in communityViewModel.VolunteerOrgs)
                        {
                            if (org.VolunteerOrgId > 0)
                            {
                                _dbContext.VolunteerOrgs.Update(org);
                            }
                            else
                            {
                                org.VolunteerExperienceId = volunteerExperience.VolunteerExperienceId;
                                org.CreatedDate = DateTime.Today;
                                org.LastModDate = DateTime.Today;
                                _dbContext.VolunteerOrgs.Add(org);
                                _dbContext.SaveChanges();

                            }

                            if (communityViewModel.VolunteerPositions.Count > 0)
                            {
                                foreach (var position in communityViewModel.VolunteerPositions)
                                {
                                   
                                    if (position.VolunteerPositionId > 0)
                                    {
                                        _dbContext.VolunteerPositions.Update(position);
                                    }
                                    else
                                    {
                                        position.VolunteerOrgId = org.VolunteerOrgId;
                                        position.CreatedDate = DateTime.Today;
                                        position.LastModDate = DateTime.Today;
                                        _dbContext.VolunteerPositions.Add(position);

                                    }
                                }
                            }
                            _dbContext.Resumes.Update(resumeProfileData);  
                        }
                        
                    }
                    _dbContext.SaveChanges();
                    transection.Commit();
                    ajaxResponse.Redirect = "/Resume/Certifications";
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw;
                }
            }
           
        
            return Json(ajaxResponse);
        }


        [HttpPost]
        public IActionResult Delete(int id, int positionId)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();

            if (id > 0 && positionId == 0)
            {
                var record = _dbContext.VolunteerOrgs.FirstOrDefault(c => c.VolunteerOrgId == id);
                if (record != null)
                {
                    var positoinData = _dbContext.VolunteerPositions.Where(c => c.VolunteerOrgId == record.VolunteerOrgId).ToList();


                    if (positoinData.Count > 0)
                    {
                        _dbContext.VolunteerPositions.RemoveRange(positoinData);
                        _dbContext.SaveChanges();

                    }
                    _dbContext.VolunteerOrgs.Remove(record);
                    _dbContext.SaveChanges();
                    ajaxResponse.Success = true;
                }

            }
            if (positionId > 0)
            {
                var position = _dbContext.VolunteerPositions.FirstOrDefault(c => c.VolunteerPositionId == positionId);
                if (position != null)
                {
                    _dbContext.VolunteerPositions.Remove(position);
                    _dbContext.SaveChanges();
                }
              
            }
            return Json(ajaxResponse);
        }
    }
}
