using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class OrganizationViewModel : CommonModel
    {
        public int OrgExperienceId { get; set; }
        public bool IsComplete { get; set; }
        public bool IsOptOut { get; set; }
        //public  List<OrgPosition> OrgPositions { get; set; } = new List<OrgPosition>();
        public List<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
