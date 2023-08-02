using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class OverseasStudyViewModel : CommonModel
    {

       public List<OverseasStudy> OverseasStudies { get; set; } = new List<OverseasStudy>();

        public bool IsComplete { get; set; }
        public bool IsOptOut { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
    }
}
