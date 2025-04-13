using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlotSystem_API.Models;

[Table("review_toggle_criteria")]
public partial class ReviewToggleCriterion
{
    [Key]
    [Column("criteria_name")]
    public string CriteriaName { get; set; } = null!;

    [Column("is_optional")]
    public bool IsOptional { get; set; }

    [ForeignKey("CriteriaName")]
    [InverseProperty("CriteriaNames")]
    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();

    [ForeignKey("CriteriaName")]
    [InverseProperty("CriteriaNames")]
    public virtual ICollection<PlotReview> Reviews { get; set; } = new List<PlotReview>();
}
