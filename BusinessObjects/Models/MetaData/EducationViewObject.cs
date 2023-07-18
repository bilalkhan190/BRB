using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class EducationViewObject
    {
        public Education Education { get; set; }
        public College College { get; set; }
        public List<AcademicHonor> AcademicHonors { get; set; } = new List<AcademicHonor>();
        public List<AcademicScholarship> AcademicScholarships { get; set; } = new List<AcademicScholarship>();
        
    }


    public class OrganizationViewObject
    {
        public OrgExperience OrgExperience { get; set; }
        public Organization Organization { get; set; }

        public List<OrgPosition> orgPositions { get; set; } = new List<OrgPosition>();
    }

    public class VolunteerViewObject
    {
        public VolunteerExperience VolunteerExperience { get; set; }
        public VolunteerOrg VolunteerOrg { get; set; }

        public List<VolunteerPosition> VolunteerPositions { get; set; } = new List<VolunteerPosition>();
    }

    public class ProfessionalViewObject
    {
        public Professional ProfessionalExperience { get; set; }
        public List<License> Licenses { get; set; } = new List<License>();
        public List<Certificate> Certificates { get; set; } = new List<Certificate>();
       
        public List<Affiliation> affilationWithPositions { get; set; } = new List<Affiliation>();
    }

    //public class AffilationWithPosition
    //{
    //    public Affiliation Affiliation { get; set; }
    //    public List<AffiliationPosition> AffiliationPositions { get; set; } = new List<AffiliationPosition>();
    //}
}
