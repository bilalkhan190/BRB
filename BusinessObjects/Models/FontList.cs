using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FontList
{
    public int FontId { get; set; }

    public string FontDesc { get; set; } = null!;

    public bool? IsActive { get; set; }
}
