using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class BuildTeam
{
    public int BuildTeamId { get; set; }

    public string Name { get; set; } = null!;

    public string? ApiKey { get; set; }

    public DateTime? ApiCreateDate { get; set; }

    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();

    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();

    public virtual ICollection<Country> CountryCodes { get; set; } = new List<Country>();

    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
