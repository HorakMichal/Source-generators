namespace MinimalApiRegistration;

internal sealed record EndpointDefinition
{
    /// <summary>
    ///		Name of the endpoint
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///		Full endpoint class namespace
    /// </summary>
    public string FullNamespace { get; set; }

    /// <summary>
    ///		Endpoint route
    /// </summary>
    public string Route { get; set; }

    /// <summary>
    ///		Endpoint group - route without endpoint name
    /// </summary>
    public string Group { get; set; }
}
