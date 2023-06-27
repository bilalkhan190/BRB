using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class CollageViewModel : CommonModel
    {
        public int CollegeId { get; set; }

        public int EducationId { get; set; }

        public string? CollegeName { get; set; }

        public string? CollegeCity { get; set; }

        public string? CollegeStateAbbr { get; set; }

        public DateTime? GradDate { get; set; }
      
        public DateTime? GradYear { get; set; }

        public string? SchoolName { get; set; }

        public int? DegreeId { get; set; }
        public string? DegreeName { get; set; }

        public string? DegreeOther { get; set; }

        public int? MajorId { get; set; }
        public string? MajorName { get; set; }

        public string? MajorOther { get; set; }

        public int? MajorSpecialtyId { get; set; }
        public string? MajorSpecialityName { get; set; }

        public string? MajorSpecialtyOther { get; set; }

        public int? MinorId { get; set; }
        public string? MinorName { get; set; }

        public string? MinorOther { get; set; }

        public int? CertificateId { get; set; }
        public string? CertificateName { get; set; }

        public string? CertificateOther { get; set; }

        public string? HonorProgram { get; set; }

        public string? Gpa { get; set; }

        public bool? IncludeGpa { get; set; }

        public bool IsComplete { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }

    }
}
