using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class SystemInfo
{
    public int SystemId { get; set; }

    public double DbVersion { get; set; }

    public double CurrentPlotVersion { get; set; }

    public DateTime LastUpdate { get; set; }

    public string? Description { get; set; }
}
