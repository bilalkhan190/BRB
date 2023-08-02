using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class ResumeService : IResumeService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public ResumeService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public Resume CreateResumeMaster(ResumeViewModels resumeViewModels)
        {
            Resume resume = new Resume();
            resume.UserId = resumeViewModels.UserId;
            resume.LastSectionVisitedId = resumeViewModels.LastSectionVisitedId;
            resume.LastSectionCompletedId = resumeViewModels.LastSectionCompletedId;
            resume.IsActive = true;
            _dbContext.Resumes.Add(resume);
            _dbContext.SaveChanges();
            resume.ResumeId = GetResumeId(resume.UserId);
            return resume;
        }
        public int GetResumeId(int userId)
        {
            return _dbContext.Resumes.FirstOrDefault(x => x.UserId == userId).ResumeId;
        }

        public UserSessionData GetResumeProfile(int userId)
        {

            var record = (from r in _dbContext.Resumes
                          join u in _dbContext.UserProfiles on r.UserId equals u.UserId
                          where r.UserId == userId
                          select new UserSessionData
                          {
                              UserName = u.UserName,
                              ResumeId = r.ResumeId,
                              UserId = r.UserId,
                              LastSectionCompletedId = r.LastSectionCompletedId,
                              LastSectionVisitedId = r.LastSectionVisitedId,
                              UserType = u.RoleType == null ? "" : u.RoleType,
                              VoucherCode = r.VoucherCode
                          }).FirstOrDefault();

            return record;
        }

        public Resume GetResumeRecordByResumeId(int resumeId)
        {
            return _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == resumeId);
        }

        public string GetStateAbbr(string stateAbbr)
        {
            return _dbContext.StateLists.FirstOrDefault(x => x.StateName == stateAbbr).StateAbbr;
        }

        public string GetStateNameByAbbr(string stateAbbr)
        {
            return _dbContext.StateLists.FirstOrDefault(x => x.StateAbbr == stateAbbr).StateName;
        }

        public bool IsUserExist(int userId)
        {
            var IsExist = _dbContext.Resumes.Any(x => x.UserId == userId);
            return IsExist;
        }

        public int UpdateResumeMaster(ResumeViewModels resumeViewModels)
        {
            var record = GetResumeRecordByResumeId(resumeViewModels.ResumeId);
            if (record != null)
            {
                record.ResumeId = resumeViewModels.ResumeId;
                record.UserId = resumeViewModels.UserId;
                record.LastSectionVisitedId = resumeViewModels.LastSectionVisitedId;
                record.LastSectionCompletedId = resumeViewModels.LastSectionCompletedId;
                record.LastModDate = resumeViewModels.LastModDate;
                record.GeneratedFileName = null; //to detect changes
                _dbContext.Entry(record).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return resumeViewModels.ResumeId;
        }

        public Resume GetResumeByPageId(int pageId)
        {
            return _dbContext.Resumes.Where(x => x.LastSectionVisitedId == pageId).FirstOrDefault();

        }
    }
}
