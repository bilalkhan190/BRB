using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace BRB.Controllers
{
    public class OrganizationController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IOrginazationService _organizationService;
        public OrganizationController(IResumeService resumeService, IOrginazationService organizationService)
        {
            _resumeService = resumeService;
            _organizationService = organizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = true;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var ids = JsonSerializer.Deserialize<TableIdentities>(sessionData.Ids);
            List<OrganizationViewObject> ListOfObjs = new List<OrganizationViewObject>();
            var record = _dbContext.Organizations.Where(x => x.OrgExperienceId == ids.orgExperienceId).ToList();
            if (record.Count > 0)
            {
                foreach (var o in record)
                {
                    OrganizationViewObject viewObject = new OrganizationViewObject();
                    viewObject.Organization = o;
                    viewObject.OrgExperienceId = ids.orgExperienceId;
                    viewObject.orgPositions = GetOrgPositions(o.OrganizationId);
                    ListOfObjs.Add(viewObject);
                }
                ajaxResponse.Data = ListOfObjs;
            }
            else
            {
                ajaxResponse.Data = _dbContext.OrgExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            }

            return Json(ajaxResponse);


        }


        private List<OrgPosition> GetOrgPositions(int orgId)
        {
            return _dbContext.OrgPositions.Where(x => x.OrganizationId == orgId).ToList();
        }

       

        [HttpPost]
        public IActionResult PostOrganizationData(OrganizationViewModel organizationViewModel)
           {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = true;
            ajaxResponse.Redirect = "/Resume/CommunityService";
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            Resume resumeProfileData = new Resume();
            resumeProfileData.ResumeId = sessionData.ResumeId;
            resumeProfileData.UserId = sessionData.UserId;
            resumeProfileData.LastSectionVisitedId = organizationViewModel.LastSectionVisitedId;
            resumeProfileData.LastModDate = DateTime.Today;
            resumeProfileData.CreatedDate = DateTime.Today;
            resumeProfileData.LastSectionCompletedId = organizationViewModel.IsComplete == true ? organizationViewModel.LastSectionVisitedId : 0;
            OrgExperience orgExperience = new OrgExperience();
            orgExperience.ResumeId = sessionData.ResumeId;
            orgExperience.CreatedDate = DateTime.Today;
            orgExperience.LastModDate = DateTime.Today;
            orgExperience.IsOptOut = false;
            orgExperience.IsComplete = organizationViewModel.IsComplete;
            using (var transection = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (organizationViewModel.OrgExperienceId > 0)
                    {
                        orgExperience.OrgExperienceId = organizationViewModel.OrgExperienceId;
                        _dbContext.OrgExperiences.Update(orgExperience);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        _dbContext.OrgExperiences.Add(orgExperience);
                        _dbContext.SaveChanges();
                    }
                    if (organizationViewModel.Organizations.Count > 0)
                    {
                        foreach (var org in organizationViewModel.Organizations)
                        {
                            org.OrgExperienceId = orgExperience.OrgExperienceId;
                            org.CreatedDate = DateTime.Today;
                            org.LastModDate = DateTime.Today;
                            if (org.OrganizationId > 0)
                            {
                                _dbContext.Organizations.Update(org);
                            }
                            else
                            {
                                _dbContext.Organizations.Add(org);
                                _dbContext.SaveChanges();   
                            }
                            if (organizationViewModel.OrgPositions.Count > 0)
                            {
                                foreach (var orgPosition in organizationViewModel.OrgPositions)
                                {
                                    orgPosition.OrganizationId = org.OrganizationId;
                                    orgPosition.CreatedDate = DateTime.Today;
                                    orgPosition.LastModDate = DateTime.Today;
                                    if (orgPosition.OrgPositionId > 0)
                                    {
                                        _dbContext.OrgPositions.Update(orgPosition);
                                    }
                                    else
                                    {
                                        _dbContext.OrgPositions.Add(orgPosition);
                                    }
                                }
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
        
           
            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult Delete(int id, int positionId)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();

            if (id > 0 && positionId == 0 )
            {
                var record = _dbContext.Organizations.FirstOrDefault(c => c.OrganizationId == id);
                if (record != null)
                {
                    var positoinData = _dbContext.OrgPositions.Where(c => c.OrganizationId == record.OrganizationId).ToList();
                  

                    if (positoinData.Count > 0)
                    {
                        _dbContext.OrgPositions.RemoveRange(positoinData);
                        _dbContext.SaveChanges();

                    }
                    _dbContext.Organizations.Remove(record);
                    _dbContext.SaveChanges();
                    ajaxResponse.Success = true;
                }

            }
            if (positionId > 0)
            {
                var position = _dbContext.OrgPositions.FirstOrDefault(c => c.OrgPositionId == positionId);
                _dbContext.OrgPositions.Remove(position);
                _dbContext.SaveChanges();
            }
            return Json(ajaxResponse);
        }
    }
}
