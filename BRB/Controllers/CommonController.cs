using BusinessObjects.Models.DTOs;
using BusinessObjects.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace BRB.Controllers
{
    public class CommonController : Controller
    {
        private readonly IDropdownService _dropdownService;
        public CommonController(IDropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        public IActionResult GetCountryList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetCountries();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetStateList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetStates();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }

        public IActionResult GetLanguageAbilityList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Data = null;
            var record = _dropdownService.GetLanguageAbility();
            if (record.Count > 0)
            {
                ajaxResponse.Data = record;
            }
            return Json(ajaxResponse);
        }
    }
}
