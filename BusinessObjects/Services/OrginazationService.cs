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
    public class OrginazationService : IOrginazationService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public OrginazationService(IMapper mapper, Wh4lprodContext dbContext)
        {
            _dbContext= dbContext;
            _mapper= mapper;
        }

        public OrgPosition AddOrganizationPosition(OrgPosition orgPosition)
        {
            _dbContext.OrgPositions.Add(orgPosition);
            _dbContext.SaveChanges();
            return orgPosition;
        }

        public OrgExperience AddOrgExperience(OrgExperience orgExperience)
        {
           _dbContext.OrgExperiences.Add(orgExperience);
            _dbContext.SaveChanges();
            return orgExperience;
        }

        public Organization AddOrginazation(Organization org)
        {
            _dbContext.Organizations.Add(org);  
            _dbContext.SaveChanges();
            return org;
        }
    }
}
