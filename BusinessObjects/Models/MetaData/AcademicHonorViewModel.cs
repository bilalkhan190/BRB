using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class AcademicHonorViewModel
    {
        public int AcademicHonorId { get; set; }

        public int CollegeId { get; set; }

        public string? HonorName { get; set; }

        public DateTime Date { get; set; }

        public string? HonorMonth { get; set; }

        public string? HonorYear { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
    }
}
