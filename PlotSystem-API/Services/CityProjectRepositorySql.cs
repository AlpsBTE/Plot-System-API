using Microsoft.EntityFrameworkCore;
using PlotSystem_API.Models;
using PlotSystem_API.Models.DTO;

namespace PlotSystem_API.Services;

public class CityProjectRepositorySql(PlotSystemContext context) : ICityProjectRepository
{
    public List<CityProjectDto> GetCityProjects(BuildTeam buildTeam)
    {
        var cities = context.CityProjects
            .Where(c => c.BuildTeams.FirstOrDefault(b => b.BuildTeamId == buildTeam.BuildTeamId) != null)
            .Include(cityProject => cityProject.CountryCodeNavigation)
            .ToList();
        
        return cities.Select(city => new CityProjectDto()
            {
                Id = city.CityProjectId,
                CountryCode = city.CountryCode,
                IsVisible = city.IsVisible,
                Material = city.CountryCodeNavigation.Material,
                CustomModelData = city.CountryCodeNavigation.CustomModelData,
                ServerName = city.ServerName,
            })
            .ToList();
    }

    public CityProjectDto? GetCityProjectById(string id)
    {
        var city = context.CityProjects
            .Include(cityProject => cityProject.CountryCodeNavigation)
            .FirstOrDefault(c => c.CityProjectId == id);
        return city == null
            ? null
            : new CityProjectDto()
            {
                Id = city.CityProjectId,
                CountryCode = city.CountryCode,
                IsVisible = city.IsVisible,
                Material = city.CountryCodeNavigation.Material,
                CustomModelData = city.CountryCodeNavigation.CustomModelData,
                ServerName = city.ServerName,
            };
    }
}