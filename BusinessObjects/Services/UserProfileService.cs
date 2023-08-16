using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly Wh4lprodContext _dbContext;
        public UserProfileService(Wh4lprodContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddData(UserProfileViewModel userProfileViewModel)
        {
            UserProfile userProfile = new UserProfile();
            userProfile.Address1 = userProfileViewModel.Address1;
            userProfile.Address2 = userProfileViewModel.Address2;
            userProfile.City = userProfileViewModel.City;
            userProfile.StateAbbr = userProfileViewModel.StateAbbr;
            userProfile.CreatedDate = DateTime.Now.Date;
            userProfile.LastModDate = DateTime.Now.Date;
            userProfile.ZipCode = userProfileViewModel.ZipCode;
            userProfile.FirstName = userProfileViewModel.FirstName;
            userProfile.LastName = userProfileViewModel.LastName;
            userProfile.Phone = userProfileViewModel.Phone;
            userProfile.UserName = userProfileViewModel.UserName;
            userProfile.UserPassword = userProfileViewModel.UserPassword;
            userProfile.IsActive = true;
            _dbContext.UserProfiles.Add(userProfile);
            _dbContext.SaveChanges();
        }

        public object GetAllIds(int resumeId)
        {
            var data = (from c in _dbContext.ContactInfos
                        from o in _dbContext.ObjectiveSummaries
                        from e in _dbContext.Educations
                        from oe in _dbContext.OverseasExperiences
                        from we in _dbContext.WorkExperiences
                        from m in _dbContext.MilitaryExperiences
                        from org in _dbContext.OrgExperiences
                        from v in _dbContext.VolunteerExperiences
                        from ts in _dbContext.TechnicalSkills
                        from p in _dbContext.Professionals
                        from ls in _dbContext.LanguageSkills
                        where c.ResumeId == resumeId && o.ResumeId == resumeId
                        && e.ResumeId == resumeId && oe.ResumeId == resumeId
                        && we.ResumeId == resumeId && m.ResumeId == resumeId
                        && org.ResumeId == resumeId && v.ResumeId == resumeId
                        && ts.ResumeId == resumeId && p.ResumeId == resumeId
                        && ls.ResumeId == resumeId
                        select new
                        {
                            contactInfoId = c.ContactInfoId,
                            objectiveId = o.ObjectiveSummaryId,
                            educationId = e.EducationId,
                            overseasExpId = oe.OverseasExperienceId,
                            orgExperience = org.OrgExperienceId,
                            workExpId = we.WorkExperienceId,
                            militaryExpId = m.MilitaryExperienceId,
                            volunteerId = v.VolunteerExperienceId,
                            technicalSkillId = ts.TechnicalSkillId,
                            professionalId = p.ProfessionalId,
                            languageSkillId = ls.LanguageSkillId
                        }).FirstOrDefault();

            return data;
        }

        public UserProfile ValidateUser(string userName, string password)
        {

            var currentUser = _dbContext.UserProfiles.FirstOrDefault(u => u.UserName == userName && u.UserPassword == password);

            return currentUser;
        }

        public bool VerifyUser(string userId)
        {
            var user = _dbContext.UserProfiles.FirstOrDefault(x => x.UserId == Convert.ToInt32(userId));
            if (user != null)
            {
                user.IsVerified = true;
                _dbContext.UserProfiles.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        public bool VerifyVoucher(string voucherCode,int userId)
        {
            
            return false;
        }

    }
}
