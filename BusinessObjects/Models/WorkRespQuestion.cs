using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class WorkRespQuestion
{
    public int WorkRespQuestionId { get; set; }

    public int PositionId { get; set; }

    public int RespQuestionId { get; set; }

    public string? Answer { get; set; }
}
