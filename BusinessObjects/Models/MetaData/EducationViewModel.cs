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
        public List<AcademicHonor> AcademicHonors { get; set; }

        public List<AcademicScholarship> AcademicScholarships { get; set; }

    

        public bool IsComplete { get; set; }
    }
}
