namespace KnowledgeMarketWebAPI.Contracts.Requests.Models;

public record RegisterModel
(
    string Login,
    string Password,
    string? Name,
    string Email
);