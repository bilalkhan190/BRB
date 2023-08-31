using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class OverseasStudy
{
    public int OverseasStudyId { get; set; }

    public int OverseasExperienceId { get; set; }

    public string? CollegeName { get; set; }

    public string? City { get; set; }

    public int? CountryId { get; set; }
    [NotMapped]
    public string? CountryName { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? EndedDate { get; set; }

    public string? ClassesCompleted { get; set; }

    public int? LivingSituationId { get; set; }
    [NotMapped]
    public string? LivingSituationName { get; set; }

    public string? LivingSituationOther { get; set; }

    public string? OtherInfo { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
