using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class WorkCompany
{
    public int CompanyId { get; set; }

    public int WorkExperienceId { get; set; }

    public string? CompanyName { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? StartMonth { get; set; }

    public int? StartYear { get; set; }

    public string? EndMonth { get; set; }

    public int? EndYear { get; set; }


    [NotMapped]
    public List<WorkPosition> Positions { get; set; } = new List<WorkPosition>();
}
