using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class OverseasStudyService : IOverseasStudyService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public OverseasStudyService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext; 
            _mapper = mapper;
        }
        public OverseasStudy AddOverseasStudies(OverseasStudyViewModel overseasStudy)
        {
            var model = _mapper.Map<OverseasStudy>(overseasStudy);
            _dbContext.OverseasStudies.Add(model);
            _dbContext.SaveChanges();   
            return model;   
           
        }

        public OverseasExperience AddOverseasExperience(OverseasExperience experience)
        {
            var model = _mapper.Map<OverseasExperience>(experience);
            _dbContext.OverseasExperiences.Add(model);
            _dbContext.SaveChanges();
            return model;
        }

        public List<LivingSituationList> GetLivingSituationList()
        {
            return _dbContext.LivingSituationLists.ToList();
        }

        public bool IsRecordExist(int resumeId)
        {
            return _dbContext.OverseasExperiences.Any(x=> x.ResumeId== resumeId);
        }

        public  OverseasExperience UpdateOverseasExperience(OverseasExperience experience)
        {
            OverseasExperience overseasExperience = new OverseasExperience();
            var model = _mapper.Map<OverseasExperience>(experience);
            _dbContext.OverseasExperiences.Update(overseasExperience);
            _dbContext.SaveChanges();
            return model;
        }

        public int GetExperienceIdByResumeId(int resumeId)
        {
            return _dbContext.OverseasExperiences.FirstOrDefault(x=>x.ResumeId == resumeId).OverseasExperienceId;
        }

        public OverseasStudy GetOverseasData(int resumeId)
        {
                        var data = (from oe in _dbContext.OverseasExperiences
                                    from os in _dbContext.OverseasStudies
                                    where oe.ResumeId == resumeId && os.OverseasExperienceId == oe.OverseasExperienceId
                                    select os).FirstOrDefault();
            return data;
        }

        public OverseasStudy UpdateOverseasStudies(OverseasStudyViewModel overseasStudy)
        {
            var model = _mapper.Map<OverseasStudy>(overseasStudy);
            _dbContext.OverseasStudies.Update(model);
            _dbContext.SaveChanges();
            return model;
        }
    }
}
