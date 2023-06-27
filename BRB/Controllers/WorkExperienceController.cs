using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BRB.Controllers
{
    public class WorkExperienceController : BaseController
    {
        private readonly IDropdownService _dropdownService;
        private readonly IWorkExperienceService _workExperienceService;
        public WorkExperienceController(IDropdownService dropdownService, IWorkExperienceService workExperienceService)
        {
            _dropdownService = dropdownService;
            _workExperienceService= workExperienceService;
        }
        public IActionResult GetJobResponsibility()
        {
            return PartialView("_WorkResponsibility");
        }
        public IActionResult GetResponsibilityFAQ(int jobCategoryId)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            ResponsibilityViewModel responsibilityViewModel = new ResponsibilityViewModel();
            var data = _workExperienceService.GetResponsibilityOptions(jobCategoryId);
                if (data != null)
            {
                responsibilityViewModel.ResponsibilityOptions = data;
            }
            var Questions = _workExperienceService.GetResponsibilityQuestions(jobCategoryId);
            if (data != null)
            {
                responsibilityViewModel.ResponsibilityQuestions = Questions;
            }
            ajaxResponse.Data = responsibilityViewModel;

            return Json(ajaxResponse);
        }

        public IActionResult GetResponsibilities()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var data = _dropdownService.GetJobCategoryList();
            if (data != null)
            {
                ajaxResponse.Data = data;
            }
            return Json(ajaxResponse);
        }

        public IActionResult PostWorkExperienceData()
        {
            return Json("");
        }
    }
}
