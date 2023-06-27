using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class StateList
{
    public string StateAbbr { get; set; } = null!;

    public string StateName { get; set; } = null!;

    public string StateCountry { get; set; } = null!;

    public bool? IsActive { get; set; }
}
