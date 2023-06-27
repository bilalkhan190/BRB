using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public DateTime GeneratedDate { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string Code { get; set; } = null!;

    public int InitialCount { get; set; }

    public int ClaimedCount { get; set; }

    public bool? IsActive { get; set; }

    public string? UrlRestriction { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Comment { get; set; }

    public DateTime LastModDate { get; set; }

    public int OfferedProductId { get; set; }
}
