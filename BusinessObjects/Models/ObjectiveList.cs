using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

public partial class ObjectiveList
{
    public int ObjectiveId { get; set; }

    public string ObjectiveDesc { get; set; } = null!;

    [NotMapped]
    public bool Checked { get; set; } = false;

    public bool? IsActive { get; set; }
}
