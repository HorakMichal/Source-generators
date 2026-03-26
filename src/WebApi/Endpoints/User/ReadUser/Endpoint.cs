using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Endpoints.User.ReadUser.Contracts;
using WebApi.Endpoints.User.ReadUser.Services;

namespace WebApi.Endpoints.User.ReadUser;

[Endpoint]
public sealed class Endpoint : IEndpoint
{
    public static RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder routeBuilder, string route) =>
        routeBuilder
            .MapGet(route, ReadUserMethod)
            .WithDescription("Read user");

    private static Ok<UserResponse> ReadUserMethod(string userId)
    {
        var service = new DataService(userId);
        var result = service.Generate();

        return TypedResults.Ok(result);
    }
}