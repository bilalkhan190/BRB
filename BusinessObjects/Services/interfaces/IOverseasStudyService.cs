using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IOverseasStudyService
    {
        OverseasExperience UpdateOverseasExperience(OverseasExperience experience);
        OverseasExperience AddOverseasExperience(OverseasExperience experience);
        OverseasStudy AddOverseasStudies(OverseasStudyViewModel overseasStudy);
        OverseasStudy UpdateOverseasStudies(OverseasStudyViewModel overseasStudy);

        List<LivingSituationList> GetLivingSituationList();
        bool IsRecordExist(int resumeId);

        OverseasStudy GetOverseasData(int resumeId);

        int GetExperienceIdByResumeId(int resumeId);



    }
}
