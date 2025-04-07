namespace PlotSystem_API.Models;

public partial class Tutorial
{
    public int TutorialId { get; set; }

    public string Uuid { get; set; } = null!;

    public int StageId { get; set; }

    public bool IsComplete { get; set; }

    public DateTime FirstStageStartDate { get; set; }

    public DateTime? LastStageCompleteDate { get; set; }
}
