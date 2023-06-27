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
    public class LanguageService : ILanguageService
    {
        private readonly Wh4lprodContext _dbContext;
        public LanguageService(Wh4lprodContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Language AddLanguage(Language language)
        {
            _dbContext.Languages.Add(language);
            _dbContext.SaveChanges();
            return language;
        }

        public LanguageSkill AddLanguageSkill(LanguageSkill skill)
        {
           _dbContext.LanguageSkills.Add(skill);
            _dbContext.SaveChanges();
            return skill;
        }

        public Language GetLanguageData(int languageId)
        {
            return _dbContext.Languages.FirstOrDefault(x => x.LanguageId == languageId);
        }

        public LanguageViewModel GetLanguageRecord(int resumeId)
        {
            var languages = (from l in _dbContext.Languages
                             join a in _dbContext.LanguageAbilityLists
                             on l.LanguageAbilityId equals a.LanguageAbilityId
                             where l.LanguageSkillId == (from ls in _dbContext.LanguageSkills
                                                         where ls.ResumeId == resumeId
                                                         select ls.LanguageSkillId
                                                                    ).SingleOrDefault()
                             select new Language
                             {
                                LanguageSkillId = l.LanguageSkillId,
                                LanguageAbilityId = l.LanguageAbilityId,
                                LanguageName = l.LanguageName,
                                LanguageId = l.LanguageId,
                                LanguageAbilityDesc = a.LanguageAbilityDesc,
                                Ability = l.Ability,
                                CreatedDate = l.CreatedDate,
                                LastModDate = l.LastModDate,
                             }).ToList();





            var languageSkills = (from langSkill in _dbContext.LanguageSkills
                                  
                                  where langSkill.ResumeId == resumeId
                                  select new LanguageViewModel
                                  {
                                      LanguageSkillId = langSkill.LanguageSkillId,
                                      ResumeId = langSkill.ResumeId,
                                      IsComplete = langSkill.IsComplete,
                                      Languages = languages
                                  }).FirstOrDefault();
            return languageSkills;

        }

        public Language UpdateLanguage(Language language)
        {
            _dbContext.Languages.Update(language);
            _dbContext.SaveChanges();
            return language;
        }

        public LanguageSkill UpdateLanguageSkill(LanguageSkill skill)
        {
            _dbContext.LanguageSkills.Update(skill);
            _dbContext.SaveChanges();
            return skill;
        }
    }
}
