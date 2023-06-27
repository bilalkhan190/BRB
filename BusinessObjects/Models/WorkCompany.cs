using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class WorkCompany
{
    public int CompanyId { get; set; }

    public int WorkExperienceId { get; set; }

    public string? CompanyName { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? StartMonth { get; set; }

    public int? StartYear { get; set; }

    public int? EndMonth { get; set; }

    public int? EndYear { get; set; }
}
