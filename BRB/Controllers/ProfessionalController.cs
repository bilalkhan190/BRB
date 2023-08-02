using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json;

namespace BRB.Controllers
{
    public class ProfessionalController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IProfessionalService _professionalService;
        public ProfessionalController(IResumeService resumeService, IProfessionalService professionalService)
        {
            _resumeService = resumeService;
            _professionalService = professionalService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            ProfessionalViewObject professionalObj = new ProfessionalViewObject();
            professionalObj.ProfessionalExperience = _dbContext.Professionals.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (professionalObj.ProfessionalExperience != null)
            {
                professionalObj.Licenses = _dbContext.Licenses.Where(x => x.ProfessionalId == professionalObj.ProfessionalExperience.ProfessionalId).ToList();
                professionalObj.Certificates = _dbContext.Certificates.Where(x => x.ProfessionalId == professionalObj.ProfessionalExperience.ProfessionalId).ToList();
                professionalObj.affilationWithPositions = _dbContext.Affiliations
                                                            .Where(x => x.ProfessionalId == professionalObj.ProfessionalExperience.ProfessionalId)
                                                            .ToList();
                professionalObj.affilationWithPositions.ForEach(x => {
                    x.AffiliationPositions = _dbContext.AffiliationPositions
                     .Where(s => s.AffiliationId == x.AffiliationId).ToList();
                });
               ajaxResponse.Data = professionalObj;
            }
           
           
            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult PostProfessionalSkillsData(ProfessionalViewModel professionalViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = true;
            ajaxResponse.Redirect = "";
            var affilationData = new Affiliation();
            var licenseData = new BusinessObjects.Models.License();    
            var certificateData = new Certificate();    
            var sessionData = JsonSerializer.Deserialize<UserSessionData>(HttpContext.Session.GetString("_userData"));
            if (sessionData != null)
            {
                ResumeViewModels resumeProfileData = new ResumeViewModels();
                resumeProfileData.ResumeId = sessionData.ResumeId;
                resumeProfileData.UserId = sessionData.UserId;
                resumeProfileData.LastSectionVisitedId = professionalViewModel.LastSectionVisitedId;
                resumeProfileData.LastModDate = DateTime.Today;
                resumeProfileData.LastSectionCompletedId = professionalViewModel.IsComplete == true ? professionalViewModel.LastSectionVisitedId : 0;
                bool isAbleToAdd = false;
                var masterData = _dbContext.Professionals.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                if (masterData == null)
                {
                    masterData = new Professional();
                    masterData.CreatedDate = DateTime.Today;
                    isAbleToAdd = true;
                }
                masterData.ResumeId = sessionData.ResumeId;  
                masterData.LastModDate = DateTime.Today;
                masterData.IsOptOut = professionalViewModel.IsOptOut;
                masterData.IsComplete = professionalViewModel.IsComplete;

                if (isAbleToAdd)
                {
                    _dbContext.Professionals.Add(masterData);                   
                }
                _dbContext.SaveChanges();

                if (professionalViewModel.Licenses.Count > 0)
                {
                    var RangeRecords = _dbContext.Licenses.Where(x => x.ProfessionalId == masterData.ProfessionalId).ToList();
                    if(RangeRecords.Count > 0)
                    {
                        _dbContext.Licenses.RemoveRange(RangeRecords);
                        _dbContext.SaveChanges();
                    }
                    foreach (var license in professionalViewModel.Licenses)
                    {
                        license.ProfessionalId = masterData.ProfessionalId;
                        license.CreatedDate = DateTime.Today;
                        license.LastModDate = DateTime.Today;
                        license.LicenseId = 0;
                        licenseData = _professionalService.AddLicense(license);
                        
                        
                    }
                    
                }
                if (professionalViewModel.Certificates.Count > 0)
                {
                    var RangeRecords = _dbContext.Certificates.Where(x => x.ProfessionalId == masterData.ProfessionalId).ToList();
                    if (RangeRecords.Count > 0)
                    {
                        _dbContext.Certificates.RemoveRange(RangeRecords);
                        _dbContext.SaveChanges();
                    }
                    foreach (var certificate in professionalViewModel.Certificates)
                    {
                        certificate.ProfessionalId = masterData.ProfessionalId;
                        certificate.CreatedDate = DateTime.Today;
                        certificate.LastModDate = DateTime.Today;
                        certificate.CertificateId = 0;
                            _professionalService.AddCertificate(certificate);
                    }
                }
                if (professionalViewModel.Affiliations.Count > 0)
                {
                    var RangeRecords = _dbContext.Affiliations.Where(x => x.ProfessionalId == masterData.ProfessionalId).ToList();
                    if (RangeRecords.Count > 0)
                    {
                        _dbContext.Affiliations.RemoveRange(RangeRecords);
                        _dbContext.SaveChanges();
                    }
                    foreach (var affilation in professionalViewModel.Affiliations)
                    {
                        affilation.ProfessionalId = masterData.ProfessionalId;
                        affilation.CreatedDate = DateTime.Today;
                        affilation.LastModDate= DateTime.Today;
                        affilation.AffiliationId = 0;
                        affilationData = _professionalService.AddAffilation(affilation);
                      
                      

                        if (professionalViewModel.AffiliationPositions.Count > 0)
                        {
                            var rangeRecord = _dbContext.AffiliationPositions.Where(x =>x.AffiliationId == affilationData.AffiliationId).ToList();
                            if (rangeRecord.Count > 0)
                            {
                                _dbContext.AffiliationPositions.RemoveRange(rangeRecord);
                                _dbContext.SaveChanges();
                            }
                            foreach (var affilationPosition in professionalViewModel.AffiliationPositions)
                            {
                                affilationPosition.AffiliationId = affilationData.AffiliationId;
                                affilationPosition.CreatedDate = DateTime.Today;
                                affilationPosition.LastModDate = DateTime.Today;
                                affilationPosition.AffiliationPositionId = 0;
                                _professionalService.AddAffilationPosition(affilationPosition);
                            }
                        }
                    }
                   
                }

                _resumeService.UpdateResumeMaster(resumeProfileData);

            }
            ajaxResponse.Redirect = "/Resume/ComputerAndTechnicalSkills";
            return Json(ajaxResponse);
        }
        [HttpPost]
        public IActionResult DeleteLicense(int id)
        {
            var record = _dbContext.Licenses.FirstOrDefault(x =>x.LicenseId == id);
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            if (record != null)
            {
                _dbContext.Licenses.Remove(record);
                _dbContext.SaveChanges();
               ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }

        public IActionResult DeleteCertificate(int id)
        {
            var record = _dbContext.Certificates.FirstOrDefault(x => x.CertificateId == id);
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            if (record != null)
            {
                _dbContext.Certificates.Remove(record);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }

        public IActionResult DeleteAffilation(int id)
        {
            var record = _dbContext.Affiliations.FirstOrDefault(x => x.AffiliationId == id);
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            if (record != null)
            {
                var position = _dbContext.AffiliationPositions.Where(x => x.AffiliationId == record.AffiliationId).ToList();
                if (position.Count > 0)
                {
                    _dbContext.AffiliationPositions.RemoveRange(position);
                    _dbContext.SaveChanges();
                }
                _dbContext.Affiliations.Remove(record);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }

        public IActionResult DeleteAffilationPosition(int id)
        {
            var record = _dbContext.AffiliationPositions.FirstOrDefault(x => x.AffiliationPositionId == id);
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            if (record != null)
            {
                _dbContext.AffiliationPositions.Remove(record);
                _dbContext.SaveChanges();
                ajaxResponse.Success = true;
            }
            return Json(ajaxResponse);
        }
    }
}
