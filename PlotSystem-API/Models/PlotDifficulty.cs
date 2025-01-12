using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class PlotDifficulty
{
    public string DifficultyId { get; set; } = null!;

    public decimal? Multiplier { get; set; }

    public int ScoreRequirement { get; set; }

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();
}
