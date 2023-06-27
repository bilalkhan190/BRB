using BusinessObjects.Models;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public  class DropdownService : IDropdownService
    {
        private readonly Wh4lprodContext _dbContaxt;
        public DropdownService(Wh4lprodContext dbContaxt)
        {
            _dbContaxt = dbContaxt;
        }

        public  List<ObjectiveList> GetObjectiveTypes()
        {
           return _dbContaxt.ObjectiveLists.ToList();
        }

        public List<YearsOfExperienceList> GetYearsOfExperience()
        {
            return _dbContaxt.YearsOfExperienceLists.ToList();
        }

        public List<ChangeTypeList> GetChangeType()
        {
            return _dbContaxt.ChangeTypeLists.ToList();
        }

        public List<PositionTypeList> GetPositionTypes()
        {
            return _dbContaxt.PositionTypeLists.ToList();
        }

        public List<DegreeList> GetDegrees()
        {
           return _dbContaxt.DegreeLists.ToList();
        }

        public List<MajorList> GetMajors()
        {
            return _dbContaxt.MajorLists.ToList();
        }

        public List<MinorList> GetMinors()
        {
            return _dbContaxt.MinorLists.ToList();
        }

        public List<MajorSpecialtyList> GetMajorSpecialties()
        {
            return _dbContaxt.MajorSpecialtyLists.ToList();
        }

        public List<StateList> GetStates()
        {
           return _dbContaxt.StateLists.ToList();
        }

        public List<CertificateList> GetCertificates()
        {
           return _dbContaxt.CertificateLists.ToList();
        }

        public List<ObjectiveList> GetObjectives()
        {
            return _dbContaxt.ObjectiveLists.ToList();
        }

        public List<CountryList> GetCountries()
        {
           return _dbContaxt.CountryLists.ToList();
        }

        public List<LanguageAbilityList> GetLanguageAbility()
        {
            return _dbContaxt.LanguageAbilityLists.ToList();
        }

        public List<JobCategoryList> GetJobCategoryList()
        {
            return _dbContaxt.JobCategoryLists.ToList();
        }
    }
}
