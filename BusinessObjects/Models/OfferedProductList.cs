using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class OfferedProductList
{
    public int OfferedProductId { get; set; }

    public string OfferedProductName { get; set; } = null!;

    public bool? IsActive { get; set; }
}
