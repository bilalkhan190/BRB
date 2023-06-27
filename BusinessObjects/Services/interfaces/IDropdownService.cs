using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IDropdownService
    {
        List<ObjectiveList> GetObjectiveTypes();
        List<YearsOfExperienceList> GetYearsOfExperience();
        List<ChangeTypeList> GetChangeType();
        List<PositionTypeList> GetPositionTypes();
        List<DegreeList> GetDegrees();
        List<MajorList> GetMajors();
        List<MinorList> GetMinors();
        List<StateList> GetStates();
        List<CertificateList> GetCertificates();
        List<MajorSpecialtyList> GetMajorSpecialties();
        List<ObjectiveList> GetObjectives();
        List<CountryList> GetCountries();
        List<LanguageAbilityList> GetLanguageAbility();
        List<JobCategoryList> GetJobCategoryList();
     
    }
}
