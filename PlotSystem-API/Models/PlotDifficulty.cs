using System.ComponentModel.DataAnnotations;

namespace PlotSystem_API.Models;

public partial class PlotDifficulty
{
    [MaxLength(255)]
    public string DifficultyId { get; set; } = null!;

    public decimal? Multiplier { get; set; }

    public int ScoreRequirement { get; set; }

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();
}
