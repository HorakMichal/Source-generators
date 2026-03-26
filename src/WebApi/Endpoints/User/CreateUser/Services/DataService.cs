using Bogus;
using WebApi.Endpoints.User.CreateUser.Contracts;

namespace WebApi.Endpoints.User.CreateUser.Services;

public sealed class DataService : Faker<CreateUserRequest>
{
    public DataService(int seed)
    {
        UseSeed(seed);
        RuleFor(x => x.Email, f => f.Internet.Email());
        RuleFor(x => x.Username, f => f.Internet.UserName());
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
    }
}
