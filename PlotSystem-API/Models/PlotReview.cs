using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class PlotReview
{
    public int ReviewId { get; set; }

    public int PlotId { get; set; }

    public string Rating { get; set; } = null!;

    public string? Feedback { get; set; }

    public string ReviewedBy { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public virtual Plot Plot { get; set; } = null!;

    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
