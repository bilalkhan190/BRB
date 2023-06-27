using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public CommunityService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public VolunteerExperience AddVolunteerExperience(VolunteerExperience volunteerExperience)
        {
            _dbContext.VolunteerExperiences.Add(volunteerExperience);
            _dbContext.SaveChanges();
            return volunteerExperience;
        }

        public VolunteerOrg AddVolunteerOrganization(VolunteerOrg volunteerOrg)
        {
            _dbContext.VolunteerOrgs.Add(volunteerOrg);
            _dbContext.SaveChanges();
            return volunteerOrg;
        }

        public VolunteerPosition AddVolunteerOrganizationPosition(VolunteerPosition volunteerPosition)
        {
            _dbContext.VolunteerPositions.Add(volunteerPosition);
            _dbContext.SaveChanges();
            return volunteerPosition;
        }
    }
}
