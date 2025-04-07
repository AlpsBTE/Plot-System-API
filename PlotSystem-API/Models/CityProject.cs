using System.ComponentModel.DataAnnotations;

namespace PlotSystem_API.Models;

public partial class CityProject
{
    [MaxLength(255)]
    public string CityProjectId { get; set; } = null!;

    public int BuildTeamId { get; set; }

    public string CountryCode { get; set; } = null!;

    [MaxLength(255)]
    public string ServerName { get; set; } = null!;

    public bool IsVisible { get; set; }

    public virtual BuildTeam BuildTeam { get; set; } = null!;

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    public virtual Server ServerNameNavigation { get; set; } = null!;
}
