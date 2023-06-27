using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IOrginazationService
    {
        OrgExperience AddOrgExperience(OrgExperience orgExperience);
        Organization  AddOrginazation(Organization org);
        OrgPosition  AddOrganizationPosition(OrgPosition orgPosition);
    }
}
