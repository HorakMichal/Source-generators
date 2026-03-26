using System.Runtime.CompilerServices;

namespace MinimalApiRegistrationTests;

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
