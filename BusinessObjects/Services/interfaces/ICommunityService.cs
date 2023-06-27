using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface ICommunityService
    {
        VolunteerExperience AddVolunteerExperience(VolunteerExperience volunteerExperience);
        VolunteerOrg AddVolunteerOrganization(VolunteerOrg volunteerOrg);
        VolunteerPosition AddVolunteerOrganizationPosition(VolunteerPosition volunteerPosition);
    }
}
