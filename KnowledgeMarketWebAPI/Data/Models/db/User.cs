using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KnowledgeMarketWebAPI.Data.Models.db;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }
    [JsonIgnore]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    [JsonIgnore]
    public virtual ICollection<PurchasedСourse> PurchasedСourses { get; set; } = new List<PurchasedСourse>();

    public virtual Role? Role { get; set; }
}
