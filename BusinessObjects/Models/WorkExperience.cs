using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class WorkExperience
{
    public int WorkExperienceId { get; set; }

    public int ResumeId { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }

    [NotMapped]
    public List<WorkCompany> Companies { get; set; } = new List<WorkCompany>();
}
