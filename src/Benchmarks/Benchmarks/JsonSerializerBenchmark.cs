using BenchmarkDotNet.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.Endpoints.User.ReadUser.Contracts;

namespace Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class JsonSerializerBenchmark
{
    private List<UserResponse> _list = null!;

    [Params(10, 100, 1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _list = Enumerable.Range(1, Size)
            .Select(i => new UserResponse
            {
                Id = i.ToString(),
                Username = $"User{i}",
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}"
            })
            .ToList();
    }

    // Normal serialization (reflection-based)
    [Benchmark(Baseline = true)]
    public string Normal() => JsonSerializer.Serialize(_list);

    // Source generator serialization (Metadata mode - serialization + deserialization)
    [Benchmark]
    public string SourceGenMetadata() => JsonSerializer.Serialize(_list, UserResponseMetadataContext.Default.ListUserResponse);

    // Source generator serialization (Serialization mode - optimized for serialization only)
    [Benchmark]
    public string SourceGenSerialize() => JsonSerializer.Serialize(_list, UserResponseSerializeContext.Default.ListUserResponse);
}

[JsonSerializable(typeof(List<UserResponse>))]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class UserResponseMetadataContext : JsonSerializerContext;

[JsonSerializable(typeof(List<UserResponse>))]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
public partial class UserResponseSerializeContext : JsonSerializerContext;