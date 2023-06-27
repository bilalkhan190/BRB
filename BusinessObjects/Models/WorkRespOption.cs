using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class WorkRespOption
{
    public int WorkRespId { get; set; }

    public int PositionId { get; set; }

    public int RespOptionId { get; set; }

    public bool Selected { get; set; }

    public string? Other { get; set; }
}
