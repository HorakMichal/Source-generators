using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebApi.OpenApiDocument;


/// <summary>
///		OpenApi document operation transformer for adding examples for WebApi endpoints
/// </summary>
internal sealed class RequestExampleOperationTransformer : IOpenApiOperationTransformer
{
	public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context,
		CancellationToken cancellationToken)
	{
		var parameter = context.Description.ParameterDescriptions
			.Select(x => x.Type)
			.Intersect(EndpointExampleExtension.AvailableTypes)
			.FirstOrDefault();

		if (parameter is null)
			return Task.CompletedTask;

		var prop = operation.RequestBody?.Content?
			.FirstOrDefault(content => (content.Value.Schema as OpenApiSchemaReference)?.Reference.Id == parameter.Name);

		if (prop is null)
			return Task.CompletedTask;

		var examples = EndpointExampleExtension.Examples
			.Where(x => x.Key == parameter)
			.ToList();

		if (examples.Count == 1)
		{
			prop.Value.Value.Example = CreateExample(examples.First().Value.ExampleRecord);

			return Task.CompletedTask;
		}

		prop.Value.Value.Examples = new Dictionary<string, IOpenApiExample>();

		foreach (var keyValuePair in examples)
		{
			var example = new OpenApiExample
			{
				Value = CreateExample(keyValuePair.Value.ExampleRecord)
			};

			prop?.Value?.Examples?.Add(keyValuePair.Value.ExampleName, example);
		}

		return Task.CompletedTask;
	}

	private static JsonNode? CreateExample(object? exampleObject)
		=> exampleObject is null
			? null
			: JsonNode.Parse(JsonSerializer.Serialize(exampleObject));
}