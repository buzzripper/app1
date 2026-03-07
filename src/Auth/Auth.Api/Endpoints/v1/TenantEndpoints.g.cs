using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Auth.Data.Entities;
using Dyvenix.App1.Auth.Api.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Endpoints.v1;

public static class TenantEndpoints
{
	public static IEndpointRouteBuilder MapTenantEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/auth/v1/tenant")
			.WithTags("Tenant");
	
		return app;
	}
}
