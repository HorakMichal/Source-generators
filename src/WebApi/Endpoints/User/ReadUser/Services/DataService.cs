using Bogus;
using WebApi.Endpoints.User.ReadUser.Contracts;

namespace WebApi.Endpoints.User.ReadUser.Services;

public sealed class DataService : Faker<UserResponse>
{
    public DataService(string userId)
    {
        UseSeed(Convert.ToByte(userId));
        RuleFor(x => x.Id, _ => userId);
        RuleFor(x => x.Username, f => f.Internet.UserName());
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
    }
}
