using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class MilitaryPosition
{
    public int MilitaryPositionId { get; set; }

    public int MilitaryExperienceId { get; set; }

    public string? Title { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public string? MainTraining { get; set; }

    public string? Responsibility1 { get; set; }

    public string? Responsibility2 { get; set; }

    public string? Responsibility3 { get; set; }

    public string? OtherInfo { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
