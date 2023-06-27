using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class License
{
    public int LicenseId { get; set; }

    public int ProfessionalId { get; set; }

    public string? Title { get; set; }

    public string? StateAbbr { get; set; }
    public string? StateName { get; set; }

    public string? ReceivedMonth { get; set; }

    public string? ReceivedYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
