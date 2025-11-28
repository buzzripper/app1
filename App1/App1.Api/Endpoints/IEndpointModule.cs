using Microsoft.AspNetCore.Routing;

namespace App1.Api.Endpoints;

public interface IEndpointModule
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
