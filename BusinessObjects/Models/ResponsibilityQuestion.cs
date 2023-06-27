using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ResponsibilityQuestion
{
    public int RespQuestionId { get; set; }

    public int ResponsibilityId { get; set; }

    public string Caption { get; set; } = null!;
}
