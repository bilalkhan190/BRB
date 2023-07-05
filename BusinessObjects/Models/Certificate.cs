using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int ProfessionalId { get; set; }

    public string? Title { get; set; }

    public string? StateAbbr { get; set; }
    [NotMapped]
    public string? StateName { get; set; }

    public string? ReceivedMonth { get; set; }

    public string? ReceivedYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
