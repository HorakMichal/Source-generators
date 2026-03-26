namespace WebApi.Endpoints;

/// <summary>
///		Interface providing method for registering endpoints
/// </summary>
public interface IEndpoint
{
    /// <summary>
    ///		Register endpoint for use
    /// </summary>
    /// <param name="routeBuilder"></param>
    /// <param name="route"></param>
    static abstract RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder routeBuilder, string route);
}
