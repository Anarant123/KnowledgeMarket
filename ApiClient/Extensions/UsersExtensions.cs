using Functional;
using KnowledgeMarketWebAPI.ApiClient;
using KnowledgeMarketWebAPI.ApiClient.Contracts.Requests;
using KnowledgeMarketWebAPI.ApiClient.Contracts.Responses;
using KnowledgeMarketWebAPI.Data.Models.db;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ApiClient.Extensions;

public static class UsersExtensions
{
    public static async Task<AuthorizedModel?> Authorize(this KnowledgeMarketApiClient api, string login, string password)
    {
        using var response = await api.HttpClient.GetAsync($"User/Authorization?login={login}&password={password}");

        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<AuthorizedModel>()
            : null;
    }

    public static async Task<IEnumerable<Error>> Registr(this KnowledgeMarketApiClient api, UserReg user)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PostAsync("User/Registration", jsonContent);

        return response.IsSuccessStatusCode
            ? Enumerable.Empty<Error>()
            : (await response.Content.ReadFromJsonAsync<List<Error>>())!;
    }

    public static async Task Recover(this KnowledgeMarketApiClient api, string login)
    {
        using var response = await api.HttpClient.PostAsync($"User/RecoveryPassword?login={login}",
            new StringContent(""));
    }

    public static async Task<User?> GetMe(this KnowledgeMarketApiClient api)
    {
        using var response = await api.HttpClient.GetAsync("User/GetMe");
        return await response.Content.ReadFromJsonAsync<User>();
    }

    public static async Task<Result<User, IEnumerable<Error>>> PersonUpdate(this KnowledgeMarketApiClient api,
        EditCourseModel person)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(person), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PutAsync("User/Update", jsonContent);

        return response.IsSuccessStatusCode
            ? new Ok<User>((await response.Content.ReadFromJsonAsync<User>())!)
            : new Error<IEnumerable<Error>>((await response.Content.ReadFromJsonAsync<List<Error>>())!);
    }

    public static async Task<bool> DeletePeople(this KnowledgeMarketApiClient api, string login)
    {
        using var response = await api.HttpClient.DeleteAsync($"User/Delete?login={login}");
        return response.IsSuccessStatusCode;
    }
}