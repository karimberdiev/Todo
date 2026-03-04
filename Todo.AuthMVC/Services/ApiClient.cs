using System.Net.Http.Headers;
using System.Net.Http.Json;
using Todo.AuthMVC.ViewModels;

namespace Todo.AuthMVC.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, string? Token, string? Message)> LoginAsync(LoginViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new
        {
            model.Username,
            model.Password
        });

        var body = await response.Content.ReadFromJsonAsync<AuthResultResponse>();
        return (response.IsSuccessStatusCode && body?.Success == true, body?.Token, body?.Message);
    }

    public async Task<(bool Success, string? Token, string? Message)> RegisterAsync(RegisterViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", new
        {
            model.Username,
            model.Email,
            model.Password
        });

        var body = await response.Content.ReadFromJsonAsync<AuthResultResponse>();
        return (response.IsSuccessStatusCode && body?.Success == true, body?.Token, body?.Message);
    }

    public async Task<List<TodoItemViewModel>> GetTasksAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var tasks = await _httpClient.GetFromJsonAsync<List<TodoItemViewModel>>("api/task");
        return tasks ?? new List<TodoItemViewModel>();
    }

    public async Task<bool> CreateTaskAsync(string token, CreateTodoViewModel model)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("api/task", new
        {
            model.Title,
            model.Description,
            model.DueDate
        });

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTaskAsync(string token, int id)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.DeleteAsync($"api/task/{id}");
        return response.IsSuccessStatusCode;
    }

    private sealed class AuthResultResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
    }
}
