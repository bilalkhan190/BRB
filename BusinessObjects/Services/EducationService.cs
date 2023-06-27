using AutoMapper;
using AutoMapper.Configuration.Conventions;
using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class EducationService : IEducationService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public EducationService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public AcademicHonor AddAcademicHonor(AcademicHonor model)
        {
            var record = _mapper.Map<AcademicHonor>(model);
         
            _dbContext.AcademicHonors.Add(record);
          
            return record;
        }
         
        public AcademicScholarship AddAcademicScholorship(AcademicScholarship model)
        {
             
           
            _dbContext.AcademicScholarships.Add(model);
            
             return model;
        }

        public College AddCollage(College college)
        {
            _dbContext.Colleges.Add(college);
            
            return college;

        }

        public Education AddEducationMaster(Education master)
        {
          _dbContext.Educations.Add(master);
       
            return master;
        }

        public College GetCollegeById(int collegeId)
        {
            return _dbContext.Colleges.Find(collegeId);
        }

        public College GetCollegeDataById(int resumeId)
        {
            var educationMasterRecord = _dbContext.Educations.FirstOrDefault(x => x.ResumeId == resumeId);
            if (educationMasterRecord != null)
            {
              return _dbContext.Colleges.FirstOrDefault(x => x.EducationId == educationMasterRecord.EducationId);
            }
            else
            {
                return null;
            }
          
            
        }

        public int GetEducationId(int resume)
        {
            return _dbContext.Educations.FirstOrDefault(x => x.ResumeId== resume).EducationId;
        }

        public Education IsRecordExist(int id)
        {
            return _dbContext.Educations.FirstOrDefault(x => x.ResumeId == id);
        }
    }
}
