using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class EducationViewModel : CommonModel
    {
        public List<College> College { get; set; }
        public List<AcademicHonor> AcademicHonors { get; set; } = new List<AcademicHonor>();

        public List<AcademicScholarship> AcademicScholarships { get; set; } = new List<AcademicScholarship>();

    

        public bool IsComplete { get; set; }
    }
}
