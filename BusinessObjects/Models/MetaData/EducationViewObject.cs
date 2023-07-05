using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class EducationViewObject
    {
        public College College { get; set; }
        public List<AcademicHonor> AcademicHonors { get; set; } = new List<AcademicHonor>();
        public List<AcademicScholarship> AcademicScholarships { get; set; } = new List<AcademicScholarship>();
        
    }


    public class OrganizationViewObject
    {
        public int OrgExperienceId { get; set; }
        public Organization Organization { get; set; }

        public List<OrgPosition> orgPositions { get; set; } = new List<OrgPosition>();
    }

    public class VolunteerViewObject
    {
        public int VolunteerExperienceId { get; set; }
        public VolunteerOrg VolunteerOrg { get; set; }

        public List<VolunteerPosition> VolunteerPositions { get; set; } = new List<VolunteerPosition>();
    }
}
