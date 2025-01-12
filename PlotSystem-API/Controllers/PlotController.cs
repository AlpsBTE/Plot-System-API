using Microsoft.AspNetCore.Mvc;
using PlotSystem_API.Models.DTO;
using PlotSystem_API.Services;

namespace PlotSystem_API.Controllers;

[Route("api/plot")]
[ApiController]
public class PlotController(IPlotRepository repository) : ControllerBase
{
    // POST: api/plot
    [HttpPost]
    public ActionResult<PlotDto> CreatePlot(
        [FromForm] string cityProjectId, 
        [FromForm] string difficultyId, 
        [FromForm] string outlineBounds,
        [FromForm] string createdBy, 
        [FromForm] IFormFile initialSchematic)
    {
        byte[]? initialSchematicBytes;
        using (var memoryStream = new MemoryStream())
        {
            initialSchematic.CopyTo(memoryStream);
            initialSchematicBytes = memoryStream.ToArray();
        }

        return repository.CreatePlot(
            cityProjectId,
            outlineBounds,
            createdBy,
            initialSchematicBytes
        );
    }
    
    // GET: api/plot/toPaste
    [HttpGet("toPaste")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<PlotDto>> GetPlotsToPaste() 
        => Ok(repository.GetPlotsToPaste());

    
    // GET: api/plot/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PlotDto> GetPlotById([FromRoute] int id)
    {
        var plot = repository.GetPlotById(id);
        return plot == null ? NotFound() : Ok(plot);
    }
    
    // GET: api/plot/{id}/setPasted
    [HttpGet("{id:int}/setPasted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult SetPasted([FromRoute] int id)
    {
        var plot = repository.GetPlotById(id);
        if (plot == null) return NotFound();
        
        repository.SetPasted(id);
        return Ok();
    }
}