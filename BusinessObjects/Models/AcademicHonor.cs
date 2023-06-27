using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class AcademicHonor
{
    public int AcademicHonorId { get; set; }

    public int CollegeId { get; set; }

    public string? HonorName { get; set; }

    public string? HonorMonth { get; set; }

    public string? HonorYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
