using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class VolunteerOrg
{
    public int VolunteerOrgId { get; set; }

    public int VolunteerExperienceId { get; set; }

    public string? VolunteerOrg1 { get; set; }

    public string? City { get; set; }

    public string? StateAbbr { get; set; }
    public string? StateName { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
