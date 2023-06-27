using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IWorkExperienceService
    {
        List<ResponsibilityOption> GetResponsibilityOptions(int jobCategoryId);
        List<ResponsibilityQuestion> GetResponsibilityQuestions(int jobCategoryId);

        WorkExperience AddWorkExperience(WorkExperience workExperience);
        WorkCompany AddWorkCompany(WorkCompany workCompany);

        WorkPosition AddWorkPosition(WorkPosition workPosition);
        WorkRespOption AddWorkResponseOption(WorkRespOption workRespOption);
        WorkRespQuestion AddWorkResponseQuestion(WorkRespQuestion workRespQuestion);


    }
}
