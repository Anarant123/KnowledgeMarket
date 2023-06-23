namespace KnowledgeMarketWebAPI.Contracts.Requests.Models;

public record AddCourseModel
(
    string Name,
    int Price,
    string Author,
    string Description,
    string Link,
    int UserId
);