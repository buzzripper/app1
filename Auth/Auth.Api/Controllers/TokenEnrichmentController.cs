using Dyvenix.Auth.Api.DTOs.EntraId;
using Dyvenix.Auth.Api.Services;
using Dyvenix.Auth.Shared;
using Dyvenix.System.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dyvenix.Auth.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
[Route("api/auth/v{version:apiVersion}/[controller]")]
[Route("api/auth/[controller]")] // Fallback route without version
public class TokenEnrichmentController : ControllerBase
{
	private readonly ITokenEnrichmentService _tokenEnrichmentService;
	private readonly ILogger<TokenEnrichmentController> _logger;

	public TokenEnrichmentController(ITokenEnrichmentService enrichmentService, ILogger<TokenEnrichmentController> logger)
	{
		_tokenEnrichmentService = enrichmentService;
		_logger = logger;
	}

	[HttpGet("[action]")]
	[AllowAnonymous]
	public async Task<ActionResult<object>> Ping()
	{
		return Ok(new PingResult(AuthConstants.ModuleId, ControllerContext.ActionDescriptor.ControllerName));
	}

	[HttpPost("[action]")]
	[Consumes("application/json")]
	[Produces("application/json")]
	public async Task<IActionResult> GetClaims([FromBody] TokenIssuanceRequest request)
	{
		try
		{
			_logger.LogInformation("==============  GETCLAIMS START  ================");

			// Optional: correlate for troubleshooting
			var corrId = Request.Headers["x-ms-client-request-id"].ToString();
			Response.Headers["x-ms-client-request-id"] = corrId;

			var response = _tokenEnrichmentService.GetClaims(request);

			// MUST return 200 and application/json
			return Ok(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error processing token issuance");

			// Return error - Entra will proceed without custom claims
			return BadRequest(new
			{
				error = "processing_error",
				error_description = "Failed to retrieve custom claims"
			});
		}
	}
}