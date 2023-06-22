using System;
using System.Collections.Generic;

namespace KnowledgeMarketWebAPI.Data.Models.db;

public partial class PurchasedСourse
{
    public int? UserId { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual User? User { get; set; }
}
