using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class CompanyJob
{
    public int CompanyJobId { get; set; }

    public int CompanyId { get; set; }

    public string? Title { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public int? CategoryId { get; set; }

    public string? SpecialProject1 { get; set; }

    public string? SpecialProject2 { get; set; }

    public string? ImprovementDesc { get; set; }

    public string? RevenueIncrease { get; set; }

    public string? ProductivityIncrease { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }

 
}
