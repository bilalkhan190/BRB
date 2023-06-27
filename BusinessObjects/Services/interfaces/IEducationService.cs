using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IEducationService
    {
        Education AddEducationMaster(Education master);

        College AddCollage(College college);
         Education IsRecordExist(int id);

        int GetEducationId(int resume);

        College GetCollegeDataById(int resumeId);
        College GetCollegeById(int collegeId);
        AcademicHonor AddAcademicHonor(AcademicHonor academicHonor);
        AcademicScholarship AddAcademicScholorship(AcademicScholarship academicScholarship);

     
    }
}
