using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class OrgExperience
{
    public int OrgExperienceId { get; set; }

    public int ResumeId { get; set; }

    public bool IsComplete { get; set; }

    public bool IsOptOut { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
