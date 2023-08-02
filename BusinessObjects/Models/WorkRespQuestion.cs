using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class WorkRespQuestion
{
    public int WorkRespQuestionId { get; set; }

    public int PositionId { get; set; }

    public int RespQuestionId { get; set; }
    [NotMapped]
    public string Question { get; set; }

    public string? Answer { get; set; }
}
