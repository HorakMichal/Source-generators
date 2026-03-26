using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace EndpointExample;

[Generator]
public class EndpointExampleGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// Marker attribute
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			$"{SourceGenerationHelper.Namespace}.Attribute.g.cs",
			SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

		// Source generator implementation
		var toGenerate = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				$"{SourceGenerationHelper.Namespace}.{SourceGenerationHelper.ContextName}Attribute",
				predicate: static (_, _) => true,
				transform: static (ctx, _) => GetRequestToGenerate(ctx))
			.Where(static m => m is not null);

		context.RegisterImplementationSourceOutput(toGenerate.Collect(), Execute);
	}

	private static EndpointExampleDefinition? GetRequestToGenerate(GeneratorAttributeSyntaxContext context)
	{
		// Get the semantic representation of the record syntax
		if (context.SemanticModel.GetDeclaredSymbol(context.TargetNode) is not IPropertySymbol recordSymbol)
		{
			// something went wrong
			return null;
		}

		var recordName = recordSymbol.Name;
		var containingTypeFullName = recordSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		var returnTypeFullName = recordSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		return new EndpointExampleDefinition
		{
			ReturnType = returnTypeFullName,
			ExampleName = recordName,
			ExamplePropertyPath = $"{containingTypeFullName}.{recordName}"
		};
	}

	private static void Execute(SourceProductionContext context, ImmutableArray<EndpointExampleDefinition?> definitions)
	{
		StringBuilder codeBuilder = new();
		codeBuilder.AppendLine(SourceGenerationHelper.ExtensionClassPrefixCode);

		foreach (var definition in definitions.Select(x => x!))
		{
			codeBuilder.AppendLine($"         //{definition.ReturnType}");
			codeBuilder.AppendLine($"""         new (typeof({definition.ReturnType}), ("{definition.ExampleName}", {definition.ExamplePropertyPath})),""");
			codeBuilder.AppendLine();
		}

		codeBuilder.AppendLine(SourceGenerationHelper.ExtensionClassSuffixCode);

		context.AddSource($"{SourceGenerationHelper.Namespace}.EndpointExtension.g.cs", codeBuilder.ToString());
	}
}