using AutoMapper;
using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using static BusinessObjects.Helper.ApplicationHelper;

namespace BRB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IResumeService _resumeService;
        public const string SessionKeyUserData = "_userData";
        private readonly IMapper _mapper;
        IWebHostEnvironment _webHostEnvironment;


        public AccountController(IUserProfileService userProfileService, IResumeService resumeService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userProfileService = userProfileService;
            _resumeService = resumeService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            System.IO.File.WriteAllText(_webHostEnvironment.WebRootPath + "/downloads/logs.txt", "Login Form Opened");
            if (HttpContext.Session.GetString(SessionKeyUserData) != null)
            {
                return RedirectToAction("Home", "Resume");
            }
            return View("SignIn");
        }

        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetString(SessionKeyUserData) != null)
            {
                return RedirectToAction("Home", "Resume");
            }
            return View("SignUp");
        }


        [Route("Verification/{id}")]
        public IActionResult AccountVerification(string id)
        {
            if (!string.IsNullOrEmpty(id) && _userProfileService.VerifyUser(id))
            {
                return View();
            }
            return View("SignIn");
        }

        [HttpPost]
        public IActionResult CreateUser(UserProfileViewModel userProfileViewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Unable to create User";
            ajaxResponse.Redirect = "";
            if (userProfileViewModel != null)
            {
                using (Wh4lprodContext _context = new Wh4lprodContext())
                {

                    //var model = _mapper.Map<UserProfile>(userProfileViewModel);
                    var model = SqlHelper.MapTo_<UserProfile>(userProfileViewModel);
                    if (model != null)
                    {
                        var userExist = _context.UserProfiles.FirstOrDefault(p => p.UserName == model.UserName);
                        if (userExist == null)
                        {
                            model.IsVerified = false;
                            model.RoleType = "";
                            _context.UserProfiles.Add(model);
                           
                            _context.SaveChanges();
                            //creating the resume master if not exist for new user
                            ResumeViewModels resumeViewModels = new ResumeViewModels();
                            resumeViewModels.UserId = model.UserId;
                            resumeViewModels.CreatedDate = DateTime.Today;
                            resumeViewModels.LastModDate = DateTime.Today;
                            var resumeData = _resumeService.CreateResumeMaster(resumeViewModels);
                            _context.ContactInfos.Add(new ContactInfo
                            {
                                Address1 = model.Address1,
                                Address2 = model.Address2,
                                City = model.City,
                                Email = model.UserName,
                                CreatedDate = DateTime.Now,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Phone = model.Phone,
                                ZipCode = model.ZipCode,
                                LastModDate = DateTime.Now,
                                StateAbbr = model.StateAbbr,
                                IsComplete = false,
                                ResumeId = resumeData.ResumeId
                            });
                            _context.SaveChanges();
                            //email verification here
                            string link = $@"{Request.Scheme}://{Request.Host}/Verification/{model.UserId}";
                            SendEmail(model.UserName, $@"{model.FirstName}, <br/><br/>Your Best Resume Builder account has been created.
You must activate it before you can start building your resume.
<a href='{link}'> Click here </a> to activate your account.", "Best Resume Builder - new account activation");
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "<p style='text-align:left !important'>Your new account has been created and an activation email has been sent to "+model.UserName+ ". Please click on the link in the email in order to activate your account.\r\n<br />To gain access to the Best Resume Builder program, please check the email address that you used for your username. It is necessary to activate your account at that email address BEFORE logging into the program. Once you have received the activation email and activated your account, you can use the serial code you received after your purchase to access the program. Go to login page.\r\n</p><p style='text-align:center !important'><br />Best Resume Builder 2.1</p>";
                            ajaxResponse.Redirect = "/Resume/ContactInfo";
                        }
                        else
                        {
                            ajaxResponse.Success = false;
                            ajaxResponse.Message = "username is already taken!";
                        }

                    }

                }

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
            ajaxResponse.Redirect = "/Resume/ContactInfo";
            if (loginViewModel != null)
            {
                var record = _userProfileService.ValidateUser(loginViewModel.UserName, loginViewModel.Password);
                if (record != null)
                {
                    //validating user
                    if (record.IsVerified == false)
                    {
                        ajaxResponse.Success = false;
                        ajaxResponse.Message = "Please verify your email";
                        return Json(ajaxResponse);
                    }

                    if (record.IsActive == false)
                    {
                        ajaxResponse.Success = false;
                        ajaxResponse.Redirect = "/Resume/VoucherVerification";
                        //return Json(ajaxResponse);
                    }
                    //else
                    //{

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
                            var resumeData = _resumeService.CreateResumeMaster(resumeViewModels);
                            sessionRecord = _resumeService.GetResumeProfile(resumeData.UserId);
                        }
                    //}


                    HttpContext.Session.SetString(SessionKeyUserData, JsonConvert.SerializeObject(sessionRecord));
                    ajaxResponse.Success = true;
                    ajaxResponse.Message = "Login successfully .. redirecting";

                    if (record.IsActive == false)
                    {
                        return Json(ajaxResponse);
                    }
                    else if (sessionRecord.UserType != "Admin")
                    {
                        if (sessionRecord != null)
                        {
                            if (sessionRecord.LastSectionCompletedId > 0)
                            {
                                switch (sessionRecord.LastSectionCompletedId)
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
                                        ajaxResponse.Redirect = "/Resume/Certifications";
                                        break;
                                    case 55:
                                        ajaxResponse.Redirect = "/Resume/ComputerAndTechnicalSkills";
                                        break;
                                    case 65:
                                        ajaxResponse.Redirect = "/Resume/LanguagesSKills";
                                        break;
                                    
                                    default: ajaxResponse.Redirect = "/Resume/Home";
                                        break;
                                }
                            }
                            else
                            {
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
                                    default: ajaxResponse.Redirect = "/Resume/Home";
                                        break;
                                }
                            }

                        }
                    }
                    else
                    {
                        ajaxResponse.Redirect = "/Admin/ProductReport";
                    }
                }
            }
            else
            {
                if (HttpContext.Session.GetString(SessionKeyUserData) != null)
                {
                    HttpContext.Session.Remove(SessionKeyUserData);
                }
                ajaxResponse.Success = false;
                ajaxResponse.Message = "user not exist";
                ajaxResponse.Redirect = "";
            }
            return Json(ajaxResponse);

        }

        public IActionResult LogOut()
        {

            HttpContext.Session.Remove(SessionKeyUserData);
            return RedirectToAction("Index");
        }

    }
}
