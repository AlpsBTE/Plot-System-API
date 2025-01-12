using PlotSystem_API.Models;
using PlotSystem_API.Models.DTO;

namespace PlotSystem_API.Services;

public interface ICityProjectRepository
{
    public List<CityProjectDto> GetCityProjects(BuildTeam buildTeam);
    public CityProjectDto? GetCityProjectById(string id);
}