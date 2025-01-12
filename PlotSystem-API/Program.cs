using Microsoft.EntityFrameworkCore;
using PlotSystem_API;
using PlotSystem_API.Models;
using PlotSystem_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var connectionString = builder.Environment.IsDevelopment() 
    ? "data source=localhost;initial catalog=plotsystem_v2;user id=root"
    : Environment.GetEnvironmentVariable("PLOTSYSTEM_DB_CONNECTION_STRING");

builder.Services.AddDbContextPool<PlotSystemContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),
        optionsBuilder => optionsBuilder.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorNumbersToAdd: null)
    )
);

builder.Services.AddScoped<ICityProjectRepository, CityProjectRepositorySql>();
builder.Services.AddScoped<IPlotRepository, PlotRepositorySql>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();