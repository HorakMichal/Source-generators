using System.Runtime.CompilerServices;

namespace EndpointExampleTests;

/// <summary>
///		Initialization of Verify and its submodules
/// </summary>
public static class VerifyConfiguration
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();

        UseSourceFileRelativeDirectory("Snapshots");
    }
}
