using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class DegreeList
{
    public int DegreeId { get; set; }

    public string DegreeDesc { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }
}
