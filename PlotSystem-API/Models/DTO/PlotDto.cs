namespace PlotSystem_API.Models.DTO;

public class PlotDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string CityProjectId { get; set; }
    public double PlotVersion { get; set; }
    public string? McVersion { get; set; }
    public byte[]? CompletedSchematic { get; set; }
}