using BusinessObjects.Models;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class WorkExperienceService : IWorkExperienceService
    {
        private readonly Wh4lprodContext _dbContext;
        public WorkExperienceService(Wh4lprodContext dbContext)
        {
            _dbContext = dbContext;
        }

        public WorkCompany AddWorkCompany(WorkCompany workCompany)
        {
            _dbContext.WorkCompanies.Add(workCompany);
            _dbContext.SaveChanges();
            return workCompany;
        }

        public WorkExperience AddWorkExperience(WorkExperience workExperience)
        {
            _dbContext.WorkExperiences.Add(workExperience);
            _dbContext.SaveChanges();
            return workExperience;
        }

        public WorkPosition AddWorkPosition(WorkPosition workPosition)
        {
            _dbContext.WorkPositions.Add(workPosition);
            _dbContext.SaveChanges();
            return workPosition;
        }

        public WorkRespOption AddWorkResponseOption(WorkRespOption workRespOption)
        {
            _dbContext.WorkRespOptions.Add(workRespOption);
            _dbContext.SaveChanges();
            return workRespOption;
        }

        public WorkRespQuestion AddWorkResponseQuestion(WorkRespQuestion workRespQuestion)
        {
            _dbContext.WorkRespQuestions.Add(workRespQuestion);
            _dbContext.SaveChanges();
            return workRespQuestion;
        }

        public List<ResponsibilityOption> GetResponsibilityOptions(int jobCategoryId)
        {
            var record = _dbContext.ResponsibilityOptions.Where(x => x.ResponsibilityId == jobCategoryId).ToList();
            return record;  
        }

        public List<ResponsibilityQuestion> GetResponsibilityQuestions(int jobCategoryId)
        {
            var record = (from rq in _dbContext.ResponsibilityQuestions
                         where rq.ResponsibilityId == jobCategoryId
                         select rq).ToList();
            return record;
        }


    }
}
