using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class CountryList
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public int? NumericalCode { get; set; }

    public string? Isoalpha { get; set; }

    public bool? IsActive { get; set; }
}
