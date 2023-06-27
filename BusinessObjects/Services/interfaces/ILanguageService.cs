using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface ILanguageService
    {
        LanguageSkill AddLanguageSkill(LanguageSkill skill);
        LanguageSkill UpdateLanguageSkill(LanguageSkill skill);
        Language AddLanguage(Language language);

        LanguageViewModel GetLanguageRecord(int resumeId);

        Language GetLanguageData(int languageId);
        Language UpdateLanguage(Language language);
    }
}
