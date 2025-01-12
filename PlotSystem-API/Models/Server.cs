using System;
using System.Collections.Generic;

namespace PlotSystem_API.Models;

public partial class Server
{
    public int BuildTeamId { get; set; }

    public string ServerName { get; set; } = null!;

    public virtual BuildTeam BuildTeam { get; set; } = null!;

    public virtual ICollection<CityProject> CityProjects { get; set; } = new List<CityProject>();
}
