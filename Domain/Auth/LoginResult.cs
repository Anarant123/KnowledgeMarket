namespace KnowledgeMarketWebAPI.Domain.Auth;

public enum LoginResult
{
    Succeeded = 0,
    Failed = 1,
    PhoneConfirmationRequired = 2
}