using EndpointExample;

namespace EndpointExampleTests;

public sealed class EndpointExampleTests
{
    [Fact]
    public async Task TestAddExample()
    {
        const string source =
            """
            using EndpointExample;

            namespace WebApi.Endpoint.User.CreateUser;

            public static class UserCreateExample
            {
            	[EndpointExample]
            	public static CreateUserExample NormalCheck => new CreateUserExample
            	{
            		Username = "Bob the Tester",
            		Email = "email@email.email",
            		FirstName = "Bob",
            		LastName = "the Tester",
            		SourceGeneratedCheck = false
            	};
            }
            """;

        var generatedCode = new EndpointExampleGenerator()
            .GenerateCode(source);

        await Verify(generatedCode);
    }

    [Fact]
    public async Task TestAddMultipleExamples()
    {
        const string source =
            """
            using EndpointExample;

            namespace WebApi.Endpoint.User.CreateUser;

            public static class UserCreateExample
            {
                [EndpointExample]
            	public static CreateUserExample NormalCheck => new CreateUserExample
            	{
            		Username = "Bob the Tester",
            		Email = "email@email.email",
            		FirstName = "Bob",
            		LastName = "the Tester",
            		SourceGeneratedCheck = false
            	};
            	
            	[EndpointExample]
                public static CreateUserExample SourceGeneratedCheck => new CreateUserExample
                {
              		    Username = "Bob the Tester",
              		    Email = "email@email.email",
              		    FirstName = "Bob",
              		    LastName = "the Tester",
              		    SourceGeneratedCheck = true
                };
            }
            """;

        var generatedCode = new EndpointExampleGenerator()
            .GenerateCode(source);

        await Verify(generatedCode);
    }
}
