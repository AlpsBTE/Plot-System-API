using Microsoft.AspNetCore.Mvc;
using PlotSystem_API.Extensions;
using PlotSystem_API.Models.DTO;
using PlotSystem_API.Services;

namespace PlotSystem_API.Controllers;

[Route("api/cityproject")]
[ApiController]
public class CityProjectController(ICityProjectRepository repository) : ControllerBase
{
    // GET: api/cityproject/
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<CityProjectDto>> GetCityProjects()
    {
        var cities = repository.GetCityProjects(HttpContext.GetBuildTeam());
        return Ok(cities);
    }
    
    // GET: api/cityproject/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CityProjectDto> GetCityProject([FromRoute] string id)
    {
        var city = repository.GetCityProjectById(id);
        return city == null ? NotFound() : Ok(city);
    }
}