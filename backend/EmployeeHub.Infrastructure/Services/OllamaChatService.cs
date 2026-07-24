using System.Text;
using System.Text.Json;
using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Application.Employees.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeHub.Infrastructure.Services;

public class OllamaChatService : IOllamaChatService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaChatService> _logger;
    private readonly IConfiguration _configuration;

    public OllamaChatService(
        HttpClient httpClient,
        ILogger<OllamaChatService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<string> AskAsync(string prompt, EmployeeDto? employee = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"AskAsync startet {prompt}");
        if (employee is null)
        {
            return "Für die KI-Zusammenfassung ist kein Mitarbeiter kontext verfügbar.";
        }

        var fallbackSummary = BuildEmployeeSummary(employee);
        _logger.LogInformation($"Fallback summary {fallbackSummary}");

        var endpoint = _configuration["Ollama:Endpoint"] ?? "http://localhost:11434/api/chat";
        var model = _configuration["Ollama:Model"] ?? "llama3.2";

        var payload = new
        {
            model,
            stream = false,
            options = new
            {
                temperature = 0.2,
                num_predict = 200
            },
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "Du bist ein HR-Assistenzsystem für EmployeeHub. Nutze ausschließlich den bereitgestellten Mitarbeiterkontext. Antworte auf Deutsch in 3 bis 5 klaren Sätzen. Formuliere eine professionelle, kompakte Zusammenfassung für ein HR-Dashboard. Erwähne Department und Skills, wenn vorhanden. Keine allgemeinen Floskeln, keine Vermutungen, keine erfundenen Informationen."
                },
                new { role = "user", content = BuildPrompt(prompt, employee) }
            }
        };

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Ollama request failed. Status: {StatusCode}. Response: {Response}", response.StatusCode, errorText);
                return fallbackSummary;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation($"Json from ollama {json}");

            using var document = JsonDocument.Parse(json);

            var answer = document.RootElement
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(answer))
            {
                return fallbackSummary;
            }

            _logger.LogInformation($"Answer from ollama {answer}");
            return answer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while calling Ollama.");
            return fallbackSummary;
        }
    }

    public static string BuildPrompt(string prompt, EmployeeDto? employee)
    {
        if (employee is null)
        {
            return prompt;
        }

        var skills = employee.Skills?.Count > 0
            ? string.Join(", ", employee.Skills.Select(s => s.SkillName ?? "Unbekannt"))
            : "Keine Skills hinterlegt";

        var builder = new StringBuilder();
        builder.AppendLine(prompt);
        builder.AppendLine();
        builder.AppendLine("Employee context:");
        builder.AppendLine($"- Name: {employee.FirstName} {employee.LastName}");
        builder.AppendLine($"- Email: {employee.Email}");
        builder.AppendLine($"- Department: {employee.Department ?? "Unbekannt"}");
        builder.AppendLine($"- Skills: {skills}");

        return builder.ToString();
    }

    public static string BuildEmployeeSummary(EmployeeDto employee)
    {
        var skills = employee.Skills?.Count > 0
            ? string.Join(", ", employee.Skills.Select(s => s.SkillName ?? "Unbekannt"))
            : "keine Skills hinterlegt";

        var department = string.IsNullOrWhiteSpace(employee.Department)
            ? "unbekannt"
            : employee.Department;

        return $"Hier ist eine kompakte HR-Zusammenfassung:\n\n" +
               $"**{employee.FirstName} {employee.LastName}**\n" +
               $"**Abteilung:** {department}\n" +
               $"**Skills:** {skills}\n" +
               $"**Kontakt:** {employee.Email}";
    }
}
