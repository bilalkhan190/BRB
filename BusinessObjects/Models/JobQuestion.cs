using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class JobQuestion
{
    public int CompanyJobId { get; set; }

    public int CategoryId { get; set; }

    public int Number { get; set; }

    public string Answer { get; set; } = null!;
}
