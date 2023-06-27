using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class MilitaryViewModel : CommonModel
    {
        public int MilitaryExperienceId { get; set; }

        public int ResumeId { get; set; }

        public string? Branch { get; set; }

        public string? City { get; set; }

        public int? CountryId { get; set; }

        public string? StartedMonth { get; set; }

        public string? StartedYear { get; set; }

        public string? EndedMonth { get; set; }

        public string? EndedYear { get; set; }

        public string? Rank { get; set; }

        public bool IsComplete { get; set; }

        public bool IsOptOut { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }

        public List<MilitaryPosition> MilitaryPositions { get; set;} = new List<MilitaryPosition>();
    }
}
