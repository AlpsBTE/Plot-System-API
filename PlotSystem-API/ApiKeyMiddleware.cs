using Microsoft.EntityFrameworkCore;
using PlotSystem_API.Models;

namespace PlotSystem_API;

public class ApiKeyMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, PlotSystemContext dbContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Unauthorized! Missing API Key.");
            return;
        }

        if (apiKey.Count != 1)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Unauthorized! Missing API Key.");
            return;
        }
        
        var buildTeam = await dbContext.BuildTeams.FirstOrDefaultAsync(b => apiKey.First() == b.ApiKey);

        if (buildTeam == null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await httpContext.Response.WriteAsync("Invalid API Key.");
            return;
        }
        
        httpContext.Items["BuildTeam"] = buildTeam;
        await next(httpContext);
    }
}