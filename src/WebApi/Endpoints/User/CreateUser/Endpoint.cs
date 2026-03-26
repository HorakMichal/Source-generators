using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Endpoints.User.CreateUser.Contracts;

namespace WebApi.Endpoints.User.CreateUser;

[Endpoint]
public sealed class Endpoint : IEndpoint
{
    public static RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder routeBuilder, string route) =>
        routeBuilder
            .MapPost(route, CreateUserMethod)
            .WithDescription("Create user");

    private static Results<Ok<bool>, BadRequest<string>> CreateUserMethod(CreateUserRequest request)
    {
       var isEmailValid = request.SourceGenerated 
           ? Services.ValidationService.CheckEmailGenerated(request.Email) 
           : Services.ValidationService.CheckEmail(request.Email);

        if (isEmailValid)
            return TypedResults.Ok(true);
        
        return TypedResults.BadRequest("Invalid email format");
    }
}