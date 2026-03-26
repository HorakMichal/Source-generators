using MinimalApiRegistration;

namespace MinimalApiRegistrationTests;

public sealed class MinimalApiRegistrationTests
{
    [Fact]
    public async Task TestAddEndpoint()
    {
        const string source =
            """
            using MinimalApiRegistration;

            namespace WebApi.Endpoints.User.ReadUser;

            [Endpoint]
            public sealed class Endpoint : IEndpoint
            {
            	public static RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder routeBuilder, string route) =>
            		routeBuilder
            			.MapGet(route, ReadUserMethod)
            			.RequireAuthorization()
            			.WithDescription("Read user");

            	private static async Task<Ok<string>> ReadUserMethod(string userId, CancellationToken cancellationToken)
            	{
            		return TypedResults.Ok(userId);
            	}
            }
            """;

        var generatedCode = new MinimalApiRegistrationGenerator()
            .GenerateCode(source);

        await Verify(generatedCode);
    }
}
