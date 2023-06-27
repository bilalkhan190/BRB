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
    public class ObjectiveService : IObjectiveService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public ObjectiveService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int AddObjectiveSummary(ObjectiveSummeryViewModel objectiveSummeryViewModel)
        {

            var model = _mapper.Map<ObjectiveSummary>(objectiveSummeryViewModel);
            _dbContext.ObjectiveSummaries.Add(model);
            _dbContext.SaveChanges();
            return model.ObjectiveSummaryId;
        }
    }
}
