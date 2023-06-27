using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class WorkPosition
{
    public int PositionId { get; set; }

    public int CompanyId { get; set; }

    public string? Title { get; set; }

    public int? StartMonth { get; set; }

    public int? StartYear { get; set; }

    public int? EndMonth { get; set; }

    public int? EndYear { get; set; }

    public int? JobResponsibilityId { get; set; }

    public string? OtherResponsibility { get; set; }
}
