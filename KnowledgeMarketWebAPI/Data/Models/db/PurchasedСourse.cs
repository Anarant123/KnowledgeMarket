using System;
using System.Collections.Generic;

namespace KnowledgeMarketWebAPI.Data.Models.db;

public partial class PurchasedСourse
{
    public int UserId { get; set; }

    public int CourseId { get; set; }

    public int Id { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public PurchasedСourse(int userId, int courseId)
    {
        UserId = userId;
        CourseId = courseId;
    }
}
