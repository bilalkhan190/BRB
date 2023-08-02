using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class ObjectiveSummary
{
    public int ObjectiveSummaryId { get; set; }
    public string? ObjectiveType { get; set; }

    public int ResumeId { get; set; }

    public int? YearsOfExperienceId { get; set; }
    [NotMapped]
    public string? YearsOfExperienceDesc { get; set; }

    public int? Objective1Id { get; set; }


    public int? Objective2Id { get; set; }

    public int? Objective3Id { get; set; }
    [NotMapped]
    public string? ObjectiveDesc1 { get; set; }
    [NotMapped]
    public string? ObjectiveDesc2 { get; set; }
    [NotMapped]
    public string? ObjectiveDesc3 { get; set; }

    public int? PositionTypeId { get; set; }
    [NotMapped]
    public string? PositionTypeDesc { get; set; }

    public string? PositionTypeOther { get; set; }

    public string? CurrentCompanyType { get; set; }

    public int? ChangeTypeId { get; set; }
    [NotMapped]
    public string? ChangeTypeDesc { get; set; }

    public string? ChangeTypeOther { get; set; }

    public string? FieldsOfExperience { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
