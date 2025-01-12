using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class BuilderHasPlot
{
    public int PlotId { get; set; }

    public string Uuid { get; set; } = null!;

    public sbyte IsOwner { get; set; }

    public virtual Plot Plot { get; set; } = null!;

    public virtual Builder Uu { get; set; } = null!;
}
