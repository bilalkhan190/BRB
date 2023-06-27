using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ChangeTypeList
{
    public int ChangeTypeId { get; set; }

    public string ChangeTypeDesc { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }
}
