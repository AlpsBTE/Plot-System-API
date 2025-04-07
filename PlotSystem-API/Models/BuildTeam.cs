using System.ComponentModel.DataAnnotations;

namespace PlotSystem_API.Models;

public partial class BuildTeam
{
    public int BuildTeamId { get; set; }

    public string Name { get; set; } = null!;

    [MaxLength(255)]
    public string? ApiKey { get; set; }

    public DateTime? ApiCreateDate { get; set; }

    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();

    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();

    public virtual ICollection<ReviewToggleCriterion> CriteriaNames { get; set; } = new List<ReviewToggleCriterion>();

    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
