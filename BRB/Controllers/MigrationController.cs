using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BusinessObjects.APIModels;

namespace BRB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        protected readonly Wh4lprodContext _dbContext;

        public MigrationController()
        {
            _dbContext = new Wh4lprodContext();
        }

        [HttpGet]
        [Route("Migrate")]
        public IActionResult Migrate(string keyword)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Data not found. Please enter email address, Phone number or User ID";
            var UserRecord = _dbContext.UserProfiles.FirstOrDefault(x => x.UserName.ToLower() == keyword.ToLower() || x.UserId.ToString() == keyword || x.Phone == keyword);
            if(UserRecord != null)
            {
                var ResumeData = _dbContext.Resumes.FirstOrDefault(x => x.UserId == UserRecord.UserId);
                if (ResumeData != null)
                {
                    var data = JsonConvert.DeserializeObject<Root>(ResumeData.Data);
                    _dbContext.ObjectiveSummaries.Add(new ObjectiveSummary
                    {
                        ChangeTypeDesc = data.objective.positiveChange,
                        
                    });
                    
                }
            }
          
            return Ok(ajaxResponse);
        }
        
    }
}
