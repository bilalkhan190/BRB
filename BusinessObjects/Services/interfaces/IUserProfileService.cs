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
    public interface IUserProfileService
    {
        void AddData(UserProfileViewModel userProfileViewModel);

        UserProfile ValidateUser(string userName, string password);

        object GetAllIds(int resumeId);
        bool VerifyUser(string userId);

    }
}
