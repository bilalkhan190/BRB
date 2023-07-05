using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class Affiliation
{
    public int AffiliationId { get; set; }

    public int ProfessionalId { get; set; }

    public string? AffiliationName { get; set; }

    public string? City { get; set; }

    public string? StateAbbr { get; set; }
    [NotMapped]
    public string? StateName { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
