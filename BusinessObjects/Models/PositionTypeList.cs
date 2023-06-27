using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class PositionTypeList
{
    public int PositionTypeId { get; set; }

    public string PositionTypeDesc { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }
}
