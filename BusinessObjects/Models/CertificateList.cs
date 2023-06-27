using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class CertificateList
{
    public int CertificateId { get; set; }

    public string CertificateDesc { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int SortOrder { get; set; }
}
