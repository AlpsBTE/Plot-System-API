namespace PlotSystem_API.Models.DTO
{
    public class CityProjectDto
    {
        public string Id { get; set; }
        public string CountryCode { get; set; }
        public bool IsVisible { get; set; }
        public string Material { get; set; }
        public string? CustomModelData { get; set; }
        public string ServerName { get; set; }
    }
}
