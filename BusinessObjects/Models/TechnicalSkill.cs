using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class TechnicalSkill
{
    public int TechnicalSkillId { get; set; }

    public int ResumeId { get; set; }

    public bool? Msword { get; set; }

    public bool? Msexcel { get; set; }

    public bool? MspowerPoint { get; set; }

    public bool? MswordPerfect { get; set; }
    public bool? GoogleSuite { get; set; }
    public bool? GoogleDocs { get; set; }

    public bool? Msoutlook { get; set; }

    public bool? MacPages { get; set; }

    public bool? MacNumbers { get; set; }

    public bool? MacKeynote { get; set; }

    public bool? AdobeAcrobat { get; set; }

    public bool? AdobePublisher { get; set; }

    public bool? AdobeIllustrator { get; set; }

    public bool? AdobePhotoshop { get; set; }

    public bool? Other { get; set; }

    public string? OtherDesc { get; set; }

    public string? OtherProgram { get; set; }

    public bool IsComplete { get; set; }

    public bool IsOptOut { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
    [NotMapped]
    public string[] microsoftPrograms { get; set; }
    [NotMapped]
    public string[] macintoshPrograms { get; set; }
    [NotMapped]
    public string[] otherPrograms { get; set; }
   
   
}
