﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class OverseasStudy
{
    public int OverseasStudyId { get; set; }

    public int OverseasExperienceId { get; set; }

    public string? CollegeName { get; set; }

    public string? City { get; set; }

    public int? CountryId { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? EndedDate { get; set; }

    public string? ClassesCompleted { get; set; }

    public int? LivingSituationId { get; set; }

    public string? LivingSituationOther { get; set; }

    public string? OtherInfo { get; set; }

    public bool IsComplete { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
