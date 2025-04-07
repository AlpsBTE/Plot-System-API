using System.ComponentModel.DataAnnotations;

namespace PlotSystem_API.Models;

public partial class ReviewToggleCriterion
{
    [MaxLength(255)]
    public string CriteriaName { get; set; } = null!;

    public bool IsOptional { get; set; }

    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();

    public virtual ICollection<PlotReview> Reviews { get; set; } = new List<PlotReview>();
}
