using System.ComponentModel.DataAnnotations;

namespace PlotSystem_API.Models;

public partial class Plot
{
    public int PlotId { get; set; }

    [MaxLength(255)]
    public string CityProjectId { get; set; } = null!;

    [MaxLength(255)]
    public string DifficultyId { get; set; } = null!;

    public string? OwnerUuid { get; set; }

    [MaxLength(255)]
    public string Status { get; set; } = null!;

    public int? Score { get; set; }
    
    public string OutlineBounds { get; set; } = null!;

    public byte[] InitialSchematic { get; set; } = null!;

    public byte[]? CompleteSchematic { get; set; }

    public DateTime? LastActivityDate { get; set; }

    public bool IsPasted { get; set; }

    public string? McVersion { get; set; }

    public double PlotVersion { get; set; }

    public int PlotType { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual CityProject CityProject { get; set; } = null!;

    public virtual PlotDifficulty Difficulty { get; set; } = null!;

    public virtual Builder? OwnerUu { get; set; }

    public virtual ICollection<PlotReview> PlotReviews { get; set; } = new List<PlotReview>();

    public virtual ICollection<Builder> Uus { get; set; } = new List<Builder>();
}
