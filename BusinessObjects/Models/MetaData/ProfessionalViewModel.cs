using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class ProfessionalViewModel : CommonModel
    {
       public List<License> Licenses { get; set; } = new List<License>();
       public List<Certificate> Certificates { get; set; } = new List<Certificate>();
        public List<Affiliation> Affiliations { get; set; } = new List<Affiliation>();
        //public List<AffiliationPosition> AffiliationPositions { get; set; } = new List<AffiliationPosition>();
        public bool IsComplete { get; set; }
        public bool IsOptOut { get; set; }
    }
}
