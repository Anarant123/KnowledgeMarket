namespace KnowledgeMarketWebAPI.Contracts.Responses;

public class Error
{
    public required string Reason { get; set; }

    public required string Message { get; set; }
}