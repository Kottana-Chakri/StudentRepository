using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureTimeTriggerFunction.HTTP_Trigger;

public class GetByIdFunction
{
    private readonly ILogger<GetByIdFunction> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GetByIdFunction(
        ILogger<GetByIdFunction> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [Function("GetByIdFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "students/{id:int}")] HttpRequest req,
        int id)
    {
        _logger.LogInformation("GetById HTTP trigger started for id: {id}", id);

        var configuredUrl = _configuration["StudentApi:GetByIdUrl"];
        var url = string.IsNullOrWhiteSpace(configuredUrl)
            ? $"{_configuration["StudentApi:GetAllUrl"]?.TrimEnd('/')}/{id}"
            : configuredUrl.Replace("{id}", id.ToString());

        if (string.IsNullOrWhiteSpace(url))
            return new BadRequestObjectResult("Student API URL is not configured.");

        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new OkObjectResult(body);

            return new ObjectResult(body)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while calling student API for id: {id}", id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}