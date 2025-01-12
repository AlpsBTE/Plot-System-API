using PlotSystem_API.Models;

namespace PlotSystem_API.Extensions;

public static class HttpContextExtensions
{
    public static BuildTeam GetBuildTeam(this HttpContext httpContext) 
        => (httpContext.Items["BuildTeam"] as BuildTeam)!;
}