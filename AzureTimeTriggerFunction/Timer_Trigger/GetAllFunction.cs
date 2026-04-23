using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureTimeTriggerFunction;

public class GetAllFunction
{
    private readonly ILogger<GetAllFunction> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GetAllFunction(
        ILogger<GetAllFunction> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [Function("GetAllStudentsTimer")]
    public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("Timer executed at: {time}", DateTime.UtcNow);

        var url = _configuration["StudentApi:GetAllUrl"];
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogError("StudentApi:GetAllUrl is missing.");
            return;
        }

        var client = _httpClientFactory.CreateClient();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Students API success: {status} | {body}", (int)response.StatusCode, body);
            else
                _logger.LogError("Students API failed: {status} | {body}", (int)response.StatusCode, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while calling Students API.");
        }

        if (myTimer.ScheduleStatus is not null)
            _logger.LogInformation("Next schedule at: {next}", myTimer.ScheduleStatus.Next);
    }
}