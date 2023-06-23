using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using KnowledgeMarketWebAPI.ApiClient;
using KnowledgeMarketWebAPI.ApiClient.Contracts.Requests;
using KnowledgeMarketWebAPI.Data.Models.db;

namespace AdBoards.ApiClient.Extensions;

public static class CoursesExtensions
{
    public static async Task<List<Course>> GetCourses(this KnowledgeMarketApiClient apiClient)
    {
        using var response = await apiClient.HttpClient.GetAsync("Courses/Get—ources");
        var courses = await response.Content.ReadFromJsonAsync<List<Course>>();

        return courses!;
    }

    public static async Task<List<Course>> GetPurchasedCourse(this KnowledgeMarketApiClient apiClient)
    {
        using var response = await apiClient.HttpClient.GetAsync("Courses/GetPurchatedCourses");
        var ads = await response.Content.ReadFromJsonAsync<List<Course>>();

        return ads!;
    }

    public static async Task<Course?> GetCourse(this KnowledgeMarketApiClient apiClient, int id)
    {
        using var response = await apiClient.HttpClient.GetAsync($"Courses/GetCourse?id={id}");

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Course>();

        return null;
    }

    public static async Task<Course?> AddCourse(this KnowledgeMarketApiClient apiClient, AddCourseModel model)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        using var response = await apiClient.HttpClient.PostAsync("Courses/Addition", jsonContent);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Course>();

        return null;
    }

    public static async Task<bool> DeleteAd(this KnowledgeMarketApiClient apiClient, int adId)
    {
        using var response = await apiClient.HttpClient.DeleteAsync($"Courses/Delete?id={adId}");

        return response.IsSuccessStatusCode;
    }

    public static async Task<Course?> CourseUpdate(this KnowledgeMarketApiClient api, AddCourseModel ad)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(ad), Encoding.UTF8, "application/json");
        using var response = await api.HttpClient.PutAsync("Courses/Update", jsonContent);

        try
        {
            var model = await response.Content.ReadFromJsonAsync<Course>();
            return model;
        }
        catch
        {
            return null;
        }
    }

    public static async Task<Course?> UpdateCoursePhoto(this KnowledgeMarketApiClient apiClient, AddCourseModel model)
    {
        if (model.Photo is null) return null;

        var stream = model.PhotoFile.OpenReadStream();
        var multipart = new MultipartFormDataContent
        {
            { new StreamContent(stream), "photo", model.PhotoFile.FileName }
        };

        using var response = await apiClient.HttpClient.PutAsync($"Courses/{model.Id}/Photo", multipart);

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<Course>();

        return null;
    }
}