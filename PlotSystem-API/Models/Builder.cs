using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class Builder
{
    public string Uuid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Score { get; set; }

    public int? FirstSlot { get; set; }

    public int? SecondSlot { get; set; }

    public int? ThirdSlot { get; set; }

    public int PlotType { get; set; }

    public virtual ICollection<BuilderHasPlot> BuilderHasPlots { get; set; } = new List<BuilderHasPlot>();

    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();

    public virtual ICollection<PlotReview> Reviews { get; set; } = new List<PlotReview>();
}
