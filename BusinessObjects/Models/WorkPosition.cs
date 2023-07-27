using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class WorkPosition
{
    public int PositionId { get; set; }

    public int CompanyId { get; set; }

    public string? Title { get; set; }

    public string? StartMonth { get; set; }

    public int? StartYear { get; set; }

    public string? EndMonth { get; set; }

    public int? EndYear { get; set; }
    public int? IncreaseRevenue { get; set; }
    public int? PercentageImprovement { get; set; }
    public string? ImproveProductivity { get; set; }
    public string? Project1 { get; set; }
    public string? Project2 { get; set; }
  

    public int? JobResponsibilityId { get; set; }

    public string? OtherResponsibility { get; set; }



    [NotMapped]
    public List<ResponsibilityOptionsResponse> responsibilityOptions { get; set; } = new List<ResponsibilityOptionsResponse>();

    [NotMapped]


    public List<WorkRespQuestion> workRespQuestions { get; set; } = new List<WorkRespQuestion>();

    [NotMapped]


    public List<JobAward> JobAwards { get; set; } = new List<JobAward>();
}
