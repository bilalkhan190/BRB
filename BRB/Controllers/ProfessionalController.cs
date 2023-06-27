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

                Professional professionalMaster = new Professional();
                professionalMaster.ResumeId = sessionData.ResumeId;
                professionalMaster.CreatedDate = DateTime.Today;
                professionalMaster.IsOptOut = false;
                professionalMaster.IsComplete = professionalViewModel.IsComplete;
            var masterData =  _professionalService.AddProfessionalMaster(professionalMaster);

                if (professionalViewModel.Licenses.Count > 0)
                {
                    foreach (var license in professionalViewModel.Licenses)
                    {
                        license.ProfessionalId = masterData.ProfessionalId;
                        license.CreatedDate = DateTime.Today;
                        licenseData =  _professionalService.AddLicense(license);
                    }
                }
                if (professionalViewModel.Certificates.Count > 0)
                {
                    foreach (var certificate in professionalViewModel.Certificates)
                    {
                        certificate.ProfessionalId = masterData.ProfessionalId;
                        _professionalService.AddCertificate(certificate);
                    }
                }
                if (professionalViewModel.Affiliations.Count > 0)
                {
                    foreach (var affilation in professionalViewModel.Affiliations)
                    {
                        affilation.ProfessionalId = masterData.ProfessionalId;
                        affilationData = _professionalService.AddAffilation(affilation);
                    }
                    if (professionalViewModel.AffiliationPositions.Count > 0)
                    {
                        foreach (var affilationPosition in professionalViewModel.AffiliationPositions)
                        {
                             affilationPosition.AffiliationId = affilationData.AffiliationId;
                             affilationPosition.CreatedDate = DateTime.Today;
                            _professionalService.AddAffilationPosition(affilationPosition);
                        }
                    }
                }


            }
            return Json(ajaxResponse);
        }
    }
}
