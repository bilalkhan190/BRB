using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Brbadmin
{
    public string UserName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
