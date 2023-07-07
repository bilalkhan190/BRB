using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
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
            var affilationData = new Affiliation();
            var licenseData = new License();    
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
                _resumeService.UpdateResumeMaster(resumeProfileData);

                var proData = _dbContext.Professionals.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                if(proData != null) {
                    _dbContext.Professionals.Remove(proData);
                    _dbContext.SaveChanges();
                }
                
                Professional professionalMaster = new Professional();
                professionalMaster.ResumeId = sessionData.ResumeId;
                professionalMaster.CreatedDate = DateTime.Today;
                professionalMaster.LastModDate = DateTime.Today;
                professionalMaster.IsOptOut = false;
                professionalMaster.IsComplete = professionalViewModel.IsComplete;
                var masterData =  _professionalService.AddProfessionalMaster(professionalMaster);

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
                        licenseData =  _professionalService.AddLicense(license);
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
                        affilationData = _professionalService.AddAffilation(affilation);

                        if (affilation.AffiliationPositions.Count > 0)
                        {
                            foreach (var affilationPosition in affilation.AffiliationPositions)
                            {
                                affilationPosition.AffiliationId = affilation.AffiliationId;
                                affilationPosition.CreatedDate = DateTime.Today;
                                _professionalService.AddAffilationPosition(affilationPosition);
                            }
                        }
                    }
                   
                }


            }
            return Json(ajaxResponse);
        }
    }
}
