using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class AcademicScholarship
{
    public int AcademicScholarshipId { get; set; }

    public int CollegeId { get; set; }

    public string? ScholarshipName { get; set; }

    public string? ScholarshipCriteria { get; set; }

    public string? ScholarshipMonth { get; set; }

    public string? ScholarshipYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
