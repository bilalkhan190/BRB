using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IResumeService
    {
        Resume CreateResumeMaster(ResumeViewModels resumeViewModels);
         Resume GetResumeRecordByResumeId(int resumeId);
        int UpdateResumeMaster(ResumeViewModels resumeViewModels);

        UserSessionData GetResumeProfile(int userId);
        bool IsUserExist(int userId);
         string GetStateAbbr(string stateAbbr);
        string GetStateNameByAbbr(string stateAbbr);
         Resume GetResumeByPageId(int pageId);



    }
}
