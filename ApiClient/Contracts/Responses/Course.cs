﻿using System.Text.Json.Serialization;

namespace KnowledgeMarketWebAPI.Data.Models.db;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public string? Author { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }
    public string? PhotoLink { get => "http://localhost:5250/" + this.Photo; }

    public int? UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Link { get; set; }
    [JsonIgnore]
    public virtual User? User { get; set; }
}
