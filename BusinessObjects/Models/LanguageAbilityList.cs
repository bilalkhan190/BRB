using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class LanguageAbilityList
{
    public int LanguageAbilityId { get; set; }

    public string LanguageAbilityDesc { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }
}
