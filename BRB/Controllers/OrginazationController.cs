using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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
            List<OrganizationViewObject> ListOfObjs = new List<OrganizationViewObject>();
            ajaxResponse.Success = true;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var masterData = _dbContext.OrgExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (masterData != null)
            {
                var record = _dbContext.Organizations.Where(x => x.OrgExperienceId == masterData.OrgExperienceId).ToList();
                if (record.Count > 0)
                {
                    foreach (var o in record)
                    {
                        o.StateName = _dbContext.StateLists.FirstOrDefault(x => x.StateAbbr == o.StateAbbr).StateName;
                        OrganizationViewObject viewObject = new OrganizationViewObject();
                        viewObject.OrgExperience = _dbContext.OrgExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                        viewObject.Organization = o;
                        viewObject.Organization.OrgPositions = GetOrgPositions(o.OrganizationId);
                        viewObject.orgPositions = GetOrgPositions(o.OrganizationId);
                        ListOfObjs.Add(viewObject);
                    }
                    ajaxResponse.Data = ListOfObjs;
                }
                if (ajaxResponse.Data == null)
                {
                    ListOfObjs.Add(new OrganizationViewObject { OrgExperience = masterData });
                    ajaxResponse.Data = ListOfObjs;
                }

            }
            else
            {
                ajaxResponse.Data = null;
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
            Resume resumeProfileData = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
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
            orgExperience.IsOptOut = organizationViewModel.IsOptOut;
            //orgExperience.IsOptOut = false;
            orgExperience.IsComplete = organizationViewModel.IsComplete;
           
            try
            {
                var record = _dbContext.OrgExperiences.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                if (record != null)
                {
                    record.ResumeId = orgExperience.ResumeId;
                    record.CreatedDate = DateTime.Today;
                    record.LastModDate = DateTime.Today;
                    record.IsComplete = organizationViewModel.IsComplete;
                    record.IsOptOut = organizationViewModel.IsOptOut;
                    _dbContext.SaveChanges();
                    orgExperience.OrgExperienceId = record.OrgExperienceId;    

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
                          
                        if (org.OrganizationId > 0)
                        {
                            org.CreatedDate = DateTime.Now;
                            org.LastModDate = DateTime.Now;
                            _dbContext.Organizations.Update(org);
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            org.OrgExperienceId = orgExperience.OrgExperienceId;
                            org.CreatedDate = DateTime.Today;
                            org.LastModDate = DateTime.Today;
                            _dbContext.Organizations.Add(org);
                            _dbContext.SaveChanges();   
                        }
                        if (org.OrgPositions.Count > 0)
                        {
                            foreach (var orgPosition in org.OrgPositions)
                            {
                                   
                                if (orgPosition.OrgPositionId > 0)
                                {
                                    orgPosition.CreatedDate = DateTime.Now;
                                    orgPosition.LastModDate= DateTime.Now;
                                    _dbContext.OrgPositions.Update(orgPosition);
                                }
                                else
                                {
                                    orgPosition.OrganizationId = org.OrganizationId;
                                    orgPosition.CreatedDate = DateTime.Today;
                                    orgPosition.LastModDate = DateTime.Today;
                                    _dbContext.OrgPositions.Add(orgPosition);
                                }
                            }
                        }
                    }
                }
                _dbContext.Resumes.Update(resumeProfileData);
                _dbContext.SaveChanges();
               
            }
            catch (Exception ex)
            {

               
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
                if (position != null)
                {
                    _dbContext.OrgPositions.Remove(position);
                    _dbContext.SaveChanges();
                }
               
            }
            return Json(ajaxResponse);
        }
    }
}
