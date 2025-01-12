using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class Tutorial
{
    public int TutorialId { get; set; }

    public string Uuid { get; set; } = null!;

    public int StageId { get; set; }

    public sbyte IsComplete { get; set; }

    public DateTime FirstStageStartDate { get; set; }

    public DateTime? LastStageCompleteDate { get; set; }
}
