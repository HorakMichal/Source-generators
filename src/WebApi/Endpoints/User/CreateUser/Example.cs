using WebApi.Endpoints.User.CreateUser.Contracts;
using WebApi.Endpoints.User.CreateUser.Services;

namespace WebApi.Endpoints.User.CreateUser;

public static class Example
{
    [EndpointExample]
    public static CreateUserRequest NormalCheck => new DataService(10).Generate() with
    {
        SourceGenerated = false
    };

    [EndpointExample]
    public static CreateUserRequest SourceGeneratedCheck => new DataService(20).Generate() with
    {
        SourceGenerated = true
    };
}
