namespace WebApi.Endpoints.User.CreateUser.Contracts;

public sealed record CreateUserRequest
{
    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public bool SourceGenerated { get; init; }
}
