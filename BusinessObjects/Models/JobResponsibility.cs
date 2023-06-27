using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class JobResponsibility
{
    public int CompanyJobId { get; set; }

    public int CategoryId { get; set; }

    public int Number { get; set; }

    public bool IsChecked { get; set; }

    public string? OtherText { get; set; }
}
