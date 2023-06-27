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
   
    public class TechnicalSkillService : ITechnicalSkillService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public TechnicalSkillService(Wh4lprodContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public TechnicalSkill AddTechnicalSkills(TechnicalSkillViewModel technicalSkill)
        {
            var model = _mapper.Map<TechnicalSkill>(technicalSkill);
           _dbContext.TechnicalSkills.Add(model);
            _dbContext.SaveChanges();
            return model;
           
        }
    }
}
