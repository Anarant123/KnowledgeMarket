using System.Text.Json.Serialization;
using KnowledgeMarketWebAPI.Data.Models.db;
using Microsoft.AspNetCore.Http;

namespace KnowledgeMarketWebAPI.ApiClient.Contracts.Requests;

public class EditCourseModel
{
    public string? Name { get; set; }

    public double? Price { get; set; }

    public string? Author { get; set; }

    public string? Description { get; set; }

    [JsonIgnore] public string Photo { get; set; } = string.Empty;

    [JsonIgnore] public IFormFile? PhotoFile { get; set; }

    public static EditCourseModel MapFromCourse(Course course)
    {
        return new EditCourseModel
        {
            Name = course.Name,
            Price = course.Price,
            Author = course.Author,
            Description = course.Description,
        };
    }
}