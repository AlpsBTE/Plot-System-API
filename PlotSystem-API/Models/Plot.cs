using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class Plot
{
    public int PlotId { get; set; }

    public string CityProjectId { get; set; } = null!;

    public string DifficultyId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? Score { get; set; }

    public string OutlineBounds { get; set; } = null!;

    public byte[] InitialSchematic { get; set; } = null!;

    public byte[]? CompleteSchematic { get; set; }

    public DateTime? LastActivityDate { get; set; }

    public sbyte IsPasted { get; set; }

    public string? McVersion { get; set; }

    public double PlotVersion { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<BuilderHasPlot> BuilderHasPlots { get; set; } = new List<BuilderHasPlot>();

    public virtual CityProject CityProject { get; set; } = null!;

    public virtual PlotDifficulty Difficulty { get; set; } = null!;

    public virtual ICollection<PlotReview> PlotReviews { get; set; } = new List<PlotReview>();
}
