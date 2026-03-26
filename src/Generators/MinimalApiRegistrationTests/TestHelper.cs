using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MinimalApiRegistrationTests;

public static class TestHelper
{
    public static GeneratorDriver GenerateCode(this IIncrementalGenerator generator, params string[] sources)
    {
        // Parse the provided string into a C# syntax tree
        var syntaxTrees = sources
            .Select(src =>
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(src);

                // Checks only basic stuff, like missing semicolons, etc.
                // Cannot check references
                var issues = syntaxTree.GetDiagnostics().ToList();
                if (issues.Count > 0)
                    throw new ArgumentException(
                        $"""
                         Source code contains following errors: 
                         {string.Join(Environment.NewLine, issues)}

                         """);

                return syntaxTree;
            });

        IEnumerable<PortableExecutableReference> references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        ];

        // Create a Roslyn compilation for the syntax tree.
        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: syntaxTrees,
            references: references);

        return CSharpGeneratorDriver
            .Create(generator)
            .RunGenerators(compilation);
    }
}