using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class CommunityViewModel : CommonModel
    {
        public List<VolunteerOrg> VolunteerOrgs { get; set; } = new List<VolunteerOrg>();
        public List<VolunteerPosition> VolunteerPositions { get; set; } = new List<VolunteerPosition>();

        public bool IsComplete { get; set; }

    }
}
