using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class College
{
    public int CollegeId { get; set; }

    public int EducationId { get; set; }

    public string? CollegeName { get; set; }

    public string? CollegeCity { get; set; }

    public string? CollegeStateAbbr { get; set; }

    public DateTime? GradDate { get; set; }

    public string? SchoolName { get; set; }

    public int? DegreeId { get; set; }
    [NotMapped]
    public string? DegreeDesc { get; set; }

    public string? DegreeOther { get; set; }

    public int? MajorId { get; set; }
    [NotMapped]
    public string? MajorDesc { get; set; }

    public string? MajorOther { get; set; }

    public int? MajorSpecialtyId { get; set; }
    [NotMapped]
    public string? MajorSpecialtyDesc { get; set; }

    public string? MajorSpecialtyOther { get; set; }

    public int? MinorId { get; set; }
    [NotMapped]
    public string? MinorDesc { get; set; }

    public string? MinorOther { get; set; }

    public int? CertificateId { get; set; }
    [NotMapped]
    public string? CertificateDesc { get; set; }

    public string? CertificateOther { get; set; }

    public string? HonorProgram { get; set; }

    public string? Gpa { get; set; }

    public bool? IncludeGpa { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
