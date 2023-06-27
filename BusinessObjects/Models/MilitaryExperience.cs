using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class MilitaryExperience  
{
    public int MilitaryExperienceId { get; set; }

    public int ResumeId { get; set; }

    public string? Branch { get; set; }

    public string? City { get; set; }

    public int? CountryId { get; set; }
    [NotMapped]
    public string? CountryName { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public string? Rank { get; set; }

    public bool IsComplete { get; set; }

    public bool IsOptOut { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
