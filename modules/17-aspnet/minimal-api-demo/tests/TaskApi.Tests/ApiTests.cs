using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskApi.Models;
using Xunit;

namespace TaskApi.Tests;

/// <summary>
/// Интеграционные тесты веб-API через WebApplicationFactory.
/// </summary>
public class ApiTests
{
    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Program>().CreateClient();

    [Fact]
    public async Task Get_ReturnsOk_WithJsonArray()
    {
        using var client = CreateClient();

        var response = await client.GetAsync("/api/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>();
        Assert.NotNull(tasks);
    }

    [Fact]
    public async Task Post_ValidTask_ReturnsCreatedAndAdds()
    {
        using var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/tasks", new { title = "Изучить ASP.NET" });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(createResponse.Headers.Location);

        var allResponse = await client.GetAsync("/api/tasks");
        var tasks = await allResponse.Content.ReadFromJsonAsync<List<TaskItem>>();
        Assert.NotNull(tasks);
        Assert.Contains(tasks!, t => t.Title == "Изучить ASP.NET");
    }

    [Fact]
    public async Task Post_EmptyTitle_ReturnsBadRequest()
    {
        using var client = CreateClient();

        var response = await client.PostAsJsonAsync("/api/tasks", new { title = "   " });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
