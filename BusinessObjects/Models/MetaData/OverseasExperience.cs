using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class OverseasExperienceViewModel : CommonModel
    {
        public int OverseasExperienceId { get; set; }

        public int ResumeId { get; set; }

        public bool IsComplete { get; set; }

        public bool IsOptOut { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
    }
}
