using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public int LanguageSkillId { get; set; }

    public string? LanguageName { get; set; }

    public string? Ability { get; set; }
    public int? LanguageAbilityId { get; set; }
    [NotMapped]
    public string? LanguageAbilityDesc { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
