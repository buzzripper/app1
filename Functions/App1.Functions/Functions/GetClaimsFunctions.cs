using Dyvenix.App1.Functions.DTOs.EntraId;
using Dyvenix.App1.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Dyvenix.App1.Functions.Functions;

public class GetClaimsFunctions
{
    private readonly ITokenEnrichmentService _tokenEnrichmentService;
    private readonly ILogger<GetClaimsFunctions> _logger;

    public GetClaimsFunctions(ITokenEnrichmentService tokenEnrichmentService, ILogger<GetClaimsFunctions> logger)
    {
        _tokenEnrichmentService = tokenEnrichmentService;
        _logger = logger;
    }

    [Function("GetClaims")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "token/GetClaims")] HttpRequest req)
    {
        try
        {
            _logger.LogInformation("==============  GETCLAIMS START  ================");

            // Correlate for troubleshooting
            var corrId = req.Headers["x-ms-client-request-id"].ToString();
            if (!string.IsNullOrEmpty(corrId))
            {
                req.HttpContext.Response.Headers["x-ms-client-request-id"] = corrId;
            }

            // Deserialize request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<TokenIssuanceRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (request == null)
            {
                _logger.LogWarning("Invalid request body");
                return new BadRequestObjectResult(new
                {
                    error = "invalid_request",
                    error_description = "Request body is required"
                });
            }

            var response = await _tokenEnrichmentService.GetClaims(request);

            // MUST return 200 and application/json
            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing token issuance");

            // Return error - Entra will proceed without custom claims
            return new BadRequestObjectResult(new
            {
                error = "processing_error",
                error_description = $"{ex.GetType().Name}: {ex.Message}"
            });
        }
    }
}
