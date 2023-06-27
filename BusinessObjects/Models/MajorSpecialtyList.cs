using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class MajorSpecialtyList
{
    public int MajorSpecialtyId { get; set; }

    public string MajorSpecialtyDesc { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int SortOrder { get; set; }
}
