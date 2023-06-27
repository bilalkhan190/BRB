using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public int WorkExperienceId { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyCity { get; set; }

    public string? CompanyStateAbbr { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
