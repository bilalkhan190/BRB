using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class MinorList
{
    public int MinorId { get; set; }

    public string MinorDesc { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int SortOrder { get; set; }
}
