// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace PlotSystem_API.Models.DTO;

public class PlotDto
{
    public required int Id { get; set; }
    public required string Status { get; set; }
    public required string CityProjectId { get; set; }
    public required double PlotVersion { get; set; }
    public string? McVersion { get; set; }
    public byte[]? CompletedSchematic { get; set; }
}