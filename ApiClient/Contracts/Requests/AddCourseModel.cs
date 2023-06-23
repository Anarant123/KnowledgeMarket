using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace KnowledgeMarketWebAPI.ApiClient.Contracts.Requests;

public class AddCourseModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public string? Author { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public int? UserId { get; set; }

    public bool? IsDeleted { get; set; }
    public string? Link { get; set; }

    [JsonIgnore] public IFormFile? PhotoFile { get; set; }
}