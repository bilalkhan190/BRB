using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public string SectionDesc { get; set; } = null!;

    public string SectionUrl { get; set; } = null!;
}
