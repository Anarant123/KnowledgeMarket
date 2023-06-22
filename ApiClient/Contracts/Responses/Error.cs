namespace KnowledgeMarketWebAPI.ApiClient.Contracts.Responses;

public class Error
{
    public string Reason { get; set; } = null!;

    public string Message { get; set; } = null!;
}