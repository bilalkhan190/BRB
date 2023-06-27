using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class MajorList
{
    public int MajorId { get; set; }

    public string MajorDesc { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int SortOrder { get; set; }
}
