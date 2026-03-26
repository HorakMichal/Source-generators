using System.Text.RegularExpressions;

namespace WebApi.Endpoints.User.CreateUser.Services;

public static partial class ValidationService
{
    public static bool CheckEmail(string email)
    {
        Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        var match = regex.Match(email);

        return match.Success;
    }

    public static bool CheckEmailGenerated(string email)
    {
        Regex regex = EmailRegex();
        var match = regex.Match(email);

        return match.Success;
    }

    [GeneratedRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
    private static partial Regex EmailRegex();
}
