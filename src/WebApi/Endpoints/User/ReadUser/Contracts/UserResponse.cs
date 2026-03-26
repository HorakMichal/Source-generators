namespace WebApi.Endpoints.User.ReadUser.Contracts;

public sealed class UserResponse
{
    public required string Id { get; init; }

    public required string Username { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }
}
