using System.ComponentModel.DataAnnotations;

namespace PlotSystem_API.Models;

public partial class Builder
{
    public string Uuid { get; set; } = null!;

    [MaxLength(255)]
    public string Name { get; set; } = null!;

    public int Score { get; set; }

    public int? FirstSlot { get; set; }

    public int? SecondSlot { get; set; }

    public int? ThirdSlot { get; set; }

    public int PlotType { get; set; }

    public virtual ICollection<Plot> PlotsNavigation { get; set; } = new List<Plot>();

    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    public virtual ICollection<PlotReview> Reviews { get; set; } = new List<PlotReview>();
}
