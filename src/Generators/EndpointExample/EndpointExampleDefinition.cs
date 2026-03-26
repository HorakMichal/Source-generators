namespace EndpointExample;

internal sealed record EndpointExampleDefinition
{
    public string ReturnType { get; set; }

    public string ExampleName { get; set; }

    public string ExamplePropertyPath { get; set; }
}