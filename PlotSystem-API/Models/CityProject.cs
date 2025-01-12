using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class CityProject
{
    public string CityProjectId { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string ServerName { get; set; } = null!;

    public bool IsVisible { get; set; }

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    public virtual Server ServerNameNavigation { get; set; } = null!;

    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();
}
