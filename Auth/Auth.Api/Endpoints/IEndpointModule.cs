using Microsoft.AspNetCore.Routing;

namespace Auth.Api.Endpoints;

public interface IEndpointModule
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
