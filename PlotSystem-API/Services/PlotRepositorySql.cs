using PlotSystem_API.Models;
using PlotSystem_API.Models.DTO;

namespace PlotSystem_API.Services
{
    public class PlotRepositorySql(PlotSystemContext context) : IPlotRepository
    {
        public PlotDto? GetPlotById(int id)
        {
            var plot = context.Plots.SingleOrDefault(p => p.PlotId == id);
            return plot == null
                ? null
                : new PlotDto()
                {
                    Id = plot.PlotId,
                    CityProjectId = plot.CityProjectId,
                    Status = plot.Status,
                    McVersion = plot.McVersion,
                    PlotVersion = plot.PlotVersion,
                    CompletedSchematic = plot.CompleteSchematic
                };
        }

        public PlotDto CreatePlot(string cityProjectId, string difficultyId, string outlineBounds, string createPlayerUuid, byte[] initialSchematic)
        {
            var plotVersion = context.SystemInfos.First().CurrentPlotVersion;
            var newPlot = new Plot()
            {
                CityProjectId = cityProjectId,
                DifficultyId = difficultyId,
                OutlineBounds = outlineBounds,
                CreatedBy = createPlayerUuid,
                InitialSchematic = initialSchematic,
                PlotVersion = plotVersion
            };
            var entity = context.Plots.Add(newPlot);
            context.SaveChanges();
            var newPlotId = newPlot.PlotId;
            
            return new PlotDto()
            {
                Id = newPlotId,
                CityProjectId = newPlot.CityProjectId,
                Status = newPlot.Status,
                PlotVersion = newPlot.PlotVersion
            };
        }

        public bool SetPasted(int id)
        {
            var plot = context.Plots.SingleOrDefault(p => p.PlotId == id);
            if (plot == null) return false;
            plot.IsPasted = true;
            return context.SaveChanges() != 0;
        }

        public List<PlotDto> GetPlotsToPaste()
        {
            var plots = context.Plots
                .Where(p => p.IsPasted == false && p.Status == "completed")
                .Take(20)
                .ToList();

            return plots.Select(plot => new PlotDto()
                {
                    Id = plot.PlotId,
                    CityProjectId = plot.CityProjectId,
                    Status = plot.Status,
                    McVersion = plot.McVersion,
                    PlotVersion = plot.PlotVersion,
                    CompletedSchematic = plot.CompleteSchematic
                })
                .ToList();
        }
    }
}
