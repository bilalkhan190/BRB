using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class YearsOfExperienceList
{
    public int YearsOfExperienceId { get; set; }

    public string YearsOfExperienceDesc { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }
}
