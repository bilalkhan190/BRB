using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class AffiliationPosition
{
    public int AffiliationPositionId { get; set; }

    public int AffiliationId { get; set; }

    public string? Title { get; set; }

    public string? StartedMonth { get; set; }

    public string? StartedYear { get; set; }

    public string? EndedMonth { get; set; }

    public string? EndedYear { get; set; }

    public string? Responsibility1 { get; set; }

    public string? Responsibility2 { get; set; }

    public string? Responsibility3 { get; set; }

    public string? OtherInfo { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
