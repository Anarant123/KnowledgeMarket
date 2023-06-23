namespace KnowledgeMarketWebAPI.Contracts.Requests.Models;

public record UpdateCourseModel
(
    int Id,
    string? Name,
    int? Price,
    string? Author,
    string? Description,
    string? Link
);