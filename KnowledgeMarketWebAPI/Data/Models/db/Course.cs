using System;
using System.Collections.Generic;

namespace KnowledgeMarketWebAPI.Data.Models.db;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public string? Author { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public int? UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }
}
