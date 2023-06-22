using KnowledgeMarketWebAPI.Data.Models.db;
using KnowledgeMarketWebAPI.Domain.Auth;

namespace KnowledgeMarketWebAPI.ApiClient.Contracts.Responses;

public class AuthorizedModel
{
    public LoginResult Kind { get; set; }

    public string Token { get; set; } = null!;

    public User User { get; set; } = null!;
}