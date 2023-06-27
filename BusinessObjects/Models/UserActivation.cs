using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class UserActivation
{
    public int UserId { get; set; }

    public string UniqueToken { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime LastModDate { get; set; }
}
