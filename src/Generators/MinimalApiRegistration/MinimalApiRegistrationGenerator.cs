using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace MinimalApiRegistration;

[Generator]
public class MinimalApiRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Marker attribute
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
            $"{SourceGenerationHelper.Namespace}.Attribute.g.cs",
            SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

        // Extensions
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
            $"{SourceGenerationHelper.Namespace}.NameExtensions.g.cs",
            SourceText.From(SourceGenerationHelper.Extension, Encoding.UTF8)));

        // Source generator implementation
        var incrementalValuesProvider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{SourceGenerationHelper.Namespace}.{SourceGenerationHelper.ContextName}Attribute",
                predicate: static (_, _) => true,
                transform: GetEndpointDefinition)
            .Where(static endpoint => endpoint is not null)
            .Select(static (endpoint, _) => endpoint!);

        context.RegisterImplementationSourceOutput(incrementalValuesProvider.Collect(), Execute);
    }

    private static EndpointDefinition? GetEndpointDefinition(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        // Get the semantic representation of the record syntax
        if (context.SemanticModel.GetDeclaredSymbol(context.TargetNode, cancellationToken) is not INamedTypeSymbol recordSymbol)
            return null; // something went wrong

        var fullNamespace = recordSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var endpointNamespaceSplit = fullNamespace.Split('.');
        var endpointName = endpointNamespaceSplit[endpointNamespaceSplit.Length - 2];
        var endpointPath = string.Join("/", endpointNamespaceSplit.Skip(2).Take(endpointNamespaceSplit.Length - 3));

        var endpointGroup = string.Join("/", endpointNamespaceSplit.Skip(2).Take(endpointNamespaceSplit.Length - 4));

        return new EndpointDefinition
        {
            Name = endpointName,
            FullNamespace = fullNamespace,
            Route = endpointPath,
            Group = endpointGroup
        };
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<EndpointDefinition> definitions)
    {
        StringBuilder codeBuilder = new();
        codeBuilder.AppendLine(SourceGenerationHelper.ExtensionClassPrefixCode);

        // Endpoint mapping
        codeBuilder.AppendLine(SourceGenerationHelper.MapAllEndpointsPrefixCode);
        foreach (var definition in definitions)
        {
            codeBuilder.AppendEndpointRegistration(definition);
        }
        codeBuilder.AppendLine(SourceGenerationHelper.MapAllEndpointsSuffixCode);

        // Create code file
        codeBuilder.AppendLine(SourceGenerationHelper.ExtensionClassSuffixCode);

        context.AddSource($"{SourceGenerationHelper.Namespace}.EndpointExtension.g.cs", codeBuilder.ToString());
    }
}
