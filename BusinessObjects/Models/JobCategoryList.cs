using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class JobCategoryList
{
    public int JobCategoryId { get; set; }

    public string JobCategoryDesc { get; set; } = null!;

    public bool? IsActive { get; set; }
}
