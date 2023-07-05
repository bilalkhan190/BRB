using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class MilitaryService : IMilitaryService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public MilitaryService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public MilitaryExperience AddMilitaryExperience(MilitaryViewModel militaryExperience, Wh4lprodContext dbContext)
        {
            var model = _mapper.Map<MilitaryExperience>(militaryExperience);
            dbContext.MilitaryExperiences.Add(model);
            dbContext.SaveChanges();
            return model;
        }

        public MilitaryPosition AddMilitaryPosition(MilitaryPosition militaryPosition, Wh4lprodContext dbContext)
        {
            dbContext.MilitaryPositions.Add(militaryPosition);
            dbContext.SaveChanges();
            return militaryPosition;
        }

        public MilitaryViewModel GetMilitaryExperienceByResumeId(int resumeId)
        {
          

            var militartPositions = (from mp in _dbContext.MilitaryPositions
                                     where mp.MilitaryExperienceId == (from me in _dbContext.MilitaryExperiences
                                                                       where me.ResumeId == resumeId
                                                                       select me.MilitaryExperienceId
                                                                       ).SingleOrDefault()
                                     select mp).ToList();


            var militaryExperience = (from me in _dbContext.MilitaryExperiences
                                                     where me.ResumeId == resumeId
                                                     select new MilitaryViewModel
                                                     {
                                                         MilitaryExperienceId = me.MilitaryExperienceId,
                                                         ResumeId = me.ResumeId,
                                                         Branch = me.Branch,
                                                         City = me.City,
                                                         IsOptOut = me.IsOptOut,
                                                         IsComplete = me.IsComplete,
                                                         CountryId = me.CountryId,
                                                         StartedMonth = me.StartedMonth,
                                                         StartedYear = me.StartedYear,
                                                         EndedMonth = me.EndedMonth,
                                                         EndedYear = me.EndedYear,
                                                         Rank = me.Rank,
                                                         MilitaryPositions = militartPositions
                                                     }).FirstOrDefault();



            return militaryExperience;
        }

        public MilitaryPosition GetMilitaryPositionByoId(int id)
        {
            return _dbContext.MilitaryPositions.FirstOrDefault(x => x.MilitaryPositionId == id);
        }

       
    }
}
