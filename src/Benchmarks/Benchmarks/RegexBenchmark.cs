using BenchmarkDotNet.Attributes;
using Bogus;
using WebApi.Endpoints.User.CreateUser.Services;

namespace Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class RegexBenchmark
{
    private string[] _emails = [];

    [Params(100, 1000)]
    public int EmailCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42); // Fixed seed for reproducibility

        var faker = new Faker();
        _emails = new string[EmailCount];

        // Generate emails
        for (int i = 0; i < EmailCount; i++)
        {
            _emails[i] = faker.Internet.Email();
        }

        // Randomly select half of the emails to invalidate
        var indicesToInvalidate = Enumerable.Range(0, EmailCount)
            .OrderBy(_ => random.Next())
            .Take(EmailCount / 2)
            .ToHashSet();

        foreach (var index in indicesToInvalidate)
        {
            _emails[index] = InvalidateEmail(_emails[index], random);
        }
    }

    private static string InvalidateEmail(string email, Random random)
    {
        return random.Next(5) switch
        {
            0 => email.Replace("@", ""),           // Remove @
            1 => email.Replace(".", ""),           // Remove dots
            2 => "@" + email,                       // Double @
            3 => email + "@extra",                  // Multiple @
            _ => "invalid email!"                   // Completely invalid
        };
    }

    [Benchmark(Baseline = true)]
    public int ReflectionRegex()
    {
        int validCount = 0;
        foreach (var email in _emails)
        {
            if (ValidationService.CheckEmail(email))
                validCount++;
        }
        return validCount;
    }

    [Benchmark]
    public int SourceGeneratedRegex()
    {
        int validCount = 0;
        foreach (var email in _emails)
        {
            if (ValidationService.CheckEmailGenerated(email))
                validCount++;
        }
        return validCount;
    }
}