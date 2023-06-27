using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class AcademicScholorshipViewModel
    {
        public int AcademicScholarshipId { get; set; }

        public int CollegeId { get; set; }

        public string? ScholarshipName { get; set; }

        public string? ScholarshipCriteria { get; set; }
        public DateTime Date { get; set; }

        public string? ScholarshipMonth { get; set; }

        public string? ScholarshipYear { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
    }
}
