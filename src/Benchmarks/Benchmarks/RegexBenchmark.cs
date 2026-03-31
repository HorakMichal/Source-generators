using BenchmarkDotNet.Attributes;
using Bogus;
using System.Text.RegularExpressions;

namespace Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class RegexBenchmark
{
    public const string Pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

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
    public int ReflectionRegexOutside()
    {
        return _emails.Count(email => new Regex(Pattern).IsMatch(email));
    }

    [Benchmark]
    public int ReflectionRegex()
    {
        Regex regex = new(Pattern);

        return _emails.Count(regex.IsMatch);
    }

    [Benchmark]
    public int SourceGeneratedRegexOutside()
    {
        return _emails.Count(email => RegexSourceGenerated.EmailRegex().IsMatch(email));
    }

    [Benchmark]
    public int SourceGeneratedRegex()
    {
        var regex = RegexSourceGenerated.EmailRegex();

        return _emails.Count(regex.IsMatch);
    }
}

public static partial class RegexSourceGenerated
{
    [GeneratedRegex(RegexBenchmark.Pattern)]
    public static partial Regex EmailRegex();
}