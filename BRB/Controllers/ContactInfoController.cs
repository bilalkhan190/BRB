using AutoMapper;
using BRB.Attributes;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;

namespace BRB.Controllers
{

   
    public class ContactInfoController : BaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IContactInfoService _contactInfoService;
        private readonly IMapper _mapper;
        public ContactInfoController(IResumeService resumeService, IContactInfoService contactInfoService, IMapper mapper)
        {
            _resumeService = resumeService;
            _contactInfoService = contactInfoService;
            _mapper = mapper;   
        }

        public IActionResult GetAllContactInfo()
        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var data = _dbContext.ContactInfos.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
            if (data != null)
            {
                ContactInfoViewModel record = _contactInfoService.GetContactInfo(sessionData.ResumeId);
                if (record != null)
                {
                    ajaxResponse.Data = record;
                }

            }
            return Json(ajaxResponse);
        }




        [HttpPost]
        public IActionResult ContactInfo(ContactInfoViewModel contactInfoViewModel)

        {
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            var contactInfo = new ContactInfo();
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = string.Empty;
            ajaxResponse.Redirect = "/Resume/Objective";
            ResumeViewModels resumeProfileData = new ResumeViewModels();
            resumeProfileData.UserId = sessionData.UserId;
            resumeProfileData.ResumeId = sessionData.ResumeId;
            resumeProfileData.LastSectionVisitedId = contactInfoViewModel.LastSectionVisitedId;
            resumeProfileData.LastSectionCompletedId = contactInfoViewModel.IsComplete == true ? contactInfoViewModel.LastSectionVisitedId : 0;
            resumeProfileData.CreatedDate = DateTime.Today;
            resumeProfileData.LastModDate = DateTime.Today;
            
          

            contactInfoViewModel.ResumeId = sessionData.ResumeId;
            contactInfoViewModel.StateAbbr = contactInfoViewModel.StateAbbr;
            contactInfoViewModel.CreatedDate = DateTime.Today;
            contactInfoViewModel.LastModDate = DateTime.Today;
            if (contactInfoViewModel.ContactInfoId == 0)
            {
                contactInfo = _contactInfoService.AddContactInfo(contactInfoViewModel);
                ajaxResponse.Success = true;
                ajaxResponse.Message = "record Added succesfully";
            }
            else
            {
                contactInfo = _contactInfoService.UpdateContactInfo(contactInfoViewModel);
                if (contactInfo.ContactInfoId > 0)
                {
                    ajaxResponse.Success = true;
                    ajaxResponse.Message = "record updated succesfully";
                }
            }

            _resumeService.UpdateResumeMaster(resumeProfileData);

            return Json(ajaxResponse);
        }
    }
}
