using Microsoft.AspNetCore.Routing;

namespace Dyvenix.System.Apis.Endpoints;

public interface IEndpointModule
{
	void MapEndpoints(IEndpointRouteBuilder app);
}
