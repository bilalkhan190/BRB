using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class ResponsibilityQuestion
{
    public int RespQuestionId { get; set; }

    public int ResponsibilityId { get; set; }

    public string Caption { get; set; } = null!;

    [NotMapped]
    public string ResponseType { get; set; } = null!;

    [NotMapped]
    public List<string> Responsibilities { get; set; } = null!;
}
