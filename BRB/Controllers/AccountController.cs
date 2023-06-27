﻿using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

namespace BRB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IResumeService _resumeService;
        public const string SessionKeyUserData = "_userData";
       

        public AccountController(IUserProfileService userProfileService, IResumeService resumeService)
        {
            _userProfileService = userProfileService;
            _resumeService = resumeService;
        }
        public IActionResult Index()
        {
            var sessionObj  =  new UserSessionData();
            HttpContext.Session.SetString(SessionKeyUserData, JsonConvert.SerializeObject(sessionObj));
            return View("SignIn");
        }

        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        [HttpPost]
        public IActionResult CreateUser(UserProfileViewModel userProfileViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Form is not subbmited";
                if (userProfileViewModel != null)
                {
                    _userProfileService.AddData(userProfileViewModel);
                    ajaxResponse.Success = true;
                    ajaxResponse.Message = "User Has been Created Successfully!";
                }
            return Json(ajaxResponse);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            var sessionRecord = new UserSessionData();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "email or password is invalid";
            ajaxResponse.Redirect = "/Resume/Home";
            if (loginViewModel != null) {
              var record =  _userProfileService.ValidateUser(loginViewModel.UserName, loginViewModel.Password);
                if (record != null)
                {
                    if (_resumeService.IsUserExist(record.UserId))
                    {
                         sessionRecord = _resumeService.GetResumeProfile(record.UserId);
                    }
                    else
                    {
                        //creating the resume master if not exist for new user
                        ResumeViewModels resumeViewModels = new ResumeViewModels();
                        resumeViewModels.UserId = record.UserId;
                        resumeViewModels.CreatedDate = DateTime.Today;
                        resumeViewModels.LastModDate = DateTime.Today;
                        var resumeData  = _resumeService.CreateResumeMaster(resumeViewModels);
                        sessionRecord = _resumeService.GetResumeProfile(resumeData.UserId);
                    }
                
                    TableIdentities identities = new TableIdentities();
                    var ds = SqlHelper.GetDataSet("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetAllIds", CommandType.StoredProcedure, new SqlParameter("ResumeId", sessionRecord.ResumeId));
                    if (ds.Tables.Count > 0)
                    {
                        identities = ds.Tables[0].ToList_<TableIdentities>().FirstOrDefault();
                        if (identities != null)
                        {
                            sessionRecord.Ids = JsonConvert.SerializeObject(identities);
                        }
                    }

                    HttpContext.Session.SetString(SessionKeyUserData, JsonConvert.SerializeObject(sessionRecord));
                        ajaxResponse.Success = true;
                        ajaxResponse.Message = "Login successfully .. redirecting";
                    if (sessionRecord != null)
                    {
                        //HttpContext.Session.SetString(SessionKeyResumeId, (sessionRecord.ResumeId.ToString()));
                        switch (sessionRecord.LastSectionVisitedId)
                        {
                            case 15:
                                ajaxResponse.Redirect = "/Resume/ContactInfo";
                                break;
                            case 20:
                                ajaxResponse.Redirect = "/Resume/Objective";
                                break;
                            case 25:
                                ajaxResponse.Redirect = "/Resume/Education";
                                break;
                            case 30:
                                ajaxResponse.Redirect = "/Resume/OverseasStudy";
                                break;
                            case 35:
                                ajaxResponse.Redirect = "/Resume/WorkExperience";
                                break;
                            case 40:
                                ajaxResponse.Redirect = "/Resume/Military";
                                break;
                            case 45:
                                ajaxResponse.Redirect = "/Resume/Organizations";
                                break;
                            case 50:
                                ajaxResponse.Redirect = "/Resume/CommunityService";
                                break;
                            case 60:
                                ajaxResponse.Redirect = "/Resume/ComputerAndTechnicalSkills";
                                break;
                            case 55:
                                ajaxResponse.Redirect = "/Resume/Certifications";
                                break;
                            case 65:
                                ajaxResponse.Redirect = "/Resume/LanguagesSKills";
                                break;

                        }
                    }
                       
                }
            }
            return Json(ajaxResponse);

        }
    }
}
