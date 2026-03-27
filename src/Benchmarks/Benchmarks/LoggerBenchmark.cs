using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using WebApi.Endpoints.User.ReadUser.Contracts;

namespace Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class LoggerBenchmark
{
    private readonly ILogger _logger;
    private readonly UserResponse _person;
    private readonly DateTime _startTime;

    public LoggerBenchmark()
    {
        // Information level enabled (debug disabled)
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
        });

        _logger = loggerFactory.CreateLogger(nameof(LoggerBenchmark));

        _person = new UserResponse
        {
            Id = "687463546",
            Username = "BobBuilder",
            FirstName = "Bob",
            LastName = "Builder"
        };
        _startTime = DateTime.UtcNow;
    }

    [Benchmark]
    public void LogInfo()
    {
        _logger.LogInformation($"Testing a message by {_person} and a {_startTime}");
    }

    [Benchmark]
    public void LogInfoParameters()
    {
        _logger.LogInformation("Testing a message by {Person} and a {StartTime}", _person, _startTime);
    }

    [Benchmark]
    public void LogInfoWithCheck()
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation($"Testing a message by {_person} and a {_startTime}");
    }

    [Benchmark]
    public void LogInfoLoggerMessage()
    {
        _logger.LogInfoLevel(_person, _startTime);
    }

    [Benchmark]
    public void LogDebug()
    {
        _logger.LogDebug($"Testing a message by {_person} and a {_startTime}");
    }

    [Benchmark]
    public void LogDebugParameters()
    {
        _logger.LogDebug("Testing a message by {Person} and a {StartTime}", _person, _startTime);
    }

    [Benchmark]
    public void LogDebugWithCheck()
    {
        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug($"Testing a message by {_person} and a {_startTime}");
    }

    [Benchmark]
    public void LogDebugLoggerMessage()
    {
        _logger.LogDebugLevel(_person, _startTime);
    }
}

public static partial class LoggerMessages
{
    private const string Message = "Testing a message by {Person} and a {Time}";

    [LoggerMessage(Level = LogLevel.Information, Message = Message)]
    public static partial void LogInfoLevel(this ILogger logger, UserResponse person, DateTime time);

    [LoggerMessage(Level = LogLevel.Debug, Message = Message)]
    public static partial void LogDebugLevel(this ILogger logger, UserResponse person, DateTime time);
}