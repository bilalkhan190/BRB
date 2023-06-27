using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class JobAward
{
    public int JobAwardId { get; set; }

    public int CompanyJobId { get; set; }

    public string? AwardDesc { get; set; }

    public string? AwardedMonth { get; set; }

    public string? AwardedYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
