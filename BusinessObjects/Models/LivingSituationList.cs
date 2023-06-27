using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class LivingSituationList
{
    public int LivingSituationId { get; set; }

    public string LivingSituationDesc { get; set; } = null!;

    public bool? IsActive { get; set; }
}
