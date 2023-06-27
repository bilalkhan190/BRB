using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class ObjectiveSummeryViewModel : CommonModel
    {
        public int ObjectiveSummaryId { get; set; }
        public string? ObjectiveType { get; set; }

        public int ResumeId { get; set; }

        public int? YearsOfExperienceId { get; set; }

        public int? Objective1Id { get; set; }

        public int? Objective2Id { get; set; }

        public int? Objective3Id { get; set; }

        public int? PositionTypeId { get; set; }

        public string? PositionTypeOther { get; set; }

        public string? CurrentCompanyType { get; set; }

        public int? ChangeTypeId { get; set; }

        public string? ChangeTypeOther { get; set; }

        public string? FieldsOfExperience { get; set; }

        public bool IsComplete { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
    }
}
