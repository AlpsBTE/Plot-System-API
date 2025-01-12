using PlotSystem_API.Models.DTO;

namespace PlotSystem_API.Services;

public interface ICityProjectRepository
{
    public List<CityProjectDto> GetCityProjects();
    public CityProjectDto? GetCityProjectById(string id);
}