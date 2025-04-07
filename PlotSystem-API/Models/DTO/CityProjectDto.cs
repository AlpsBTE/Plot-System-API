// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace PlotSystem_API.Models.DTO
{
    public class CityProjectDto
    {
        public required string Id { get; set; }
        public required string CountryCode { get; set; }
        public required bool IsVisible { get; set; }
        public required string Material { get; set; }
        public string? CustomModelData { get; set; }
        public required string ServerName { get; set; }
    }
}
