using PlotSystem_API.Models.DTO;

namespace PlotSystem_API.Services;

public interface IPlotRepository
{
    public PlotDto? GetPlotById(int id);
    public PlotDto CreatePlot(string cityProjectId, string outlineBounds, string createPlayerUuid, byte[] initialSchematic);
    public bool SetPasted(int id);
    public List<PlotDto> GetPlotsToPaste();
}