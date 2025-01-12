using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class Country
{
    public string CountryCode { get; set; } = null!;

    public string Continent { get; set; } = null!;

    public string Material { get; set; } = null!;

    public string? CustomModelData { get; set; }

    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();

    public virtual ICollection<BuildTeam> BuildTeams { get; set; } = new List<BuildTeam>();
}
