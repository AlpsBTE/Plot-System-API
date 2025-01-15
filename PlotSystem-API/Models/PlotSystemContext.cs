using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PlotSystem_API.Models;

public partial class PlotSystemContext : DbContext
{
    public PlotSystemContext()
    {
    }

    public PlotSystemContext(DbContextOptions<PlotSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuildTeam> BuildTeams { get; set; }

    public virtual DbSet<Builder> Builders { get; set; }

    public virtual DbSet<BuilderHasPlot> BuilderHasPlots { get; set; }

    public virtual DbSet<CityProject> CityProjects { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Plot> Plots { get; set; }

    public virtual DbSet<PlotDifficulty> PlotDifficulties { get; set; }

    public virtual DbSet<PlotReview> PlotReviews { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<SystemInfo> SystemInfos { get; set; }

    public virtual DbSet<Tutorial> Tutorials { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BuildTeam>(entity =>
        {
            entity.HasKey(e => e.BuildTeamId).HasName("PRIMARY");

            entity.ToTable("build_team");

            entity.HasIndex(e => e.ApiKey, "api_key").IsUnique();

            entity.Property(e => e.BuildTeamId)
                .HasColumnType("int(11)")
                .HasColumnName("build_team_id");
            entity.Property(e => e.ApiCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("api_create_date");
            entity.Property(e => e.ApiKey).HasColumnName("api_key");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasMany(d => d.CityProjects).WithMany(p => p.BuildTeams)
                .UsingEntity<Dictionary<string, object>>(
                    "BuildTeamHasCityProject",
                    r => r.HasOne<CityProject>().WithMany()
                        .HasForeignKey("CityProjectId")
                        .HasConstraintName("build_team_has_city_project_ibfk_2"),
                    l => l.HasOne<BuildTeam>().WithMany()
                        .HasForeignKey("BuildTeamId")
                        .HasConstraintName("build_team_has_city_project_ibfk_1"),
                    j =>
                    {
                        j.HasKey("BuildTeamId", "CityProjectId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("build_team_has_city_project");
                        j.HasIndex(new[] { "CityProjectId" }, "city_project_id");
                        j.IndexerProperty<int>("BuildTeamId")
                            .HasColumnType("int(11)")
                            .HasColumnName("build_team_id");
                        j.IndexerProperty<string>("CityProjectId").HasColumnName("city_project_id");
                    });

            entity.HasMany(d => d.CountryCodes).WithMany(p => p.BuildTeams)
                .UsingEntity<Dictionary<string, object>>(
                    "BuildTeamHasCountry",
                    r => r.HasOne<Country>().WithMany()
                        .HasForeignKey("CountryCode")
                        .HasConstraintName("build_team_has_country_ibfk_2"),
                    l => l.HasOne<BuildTeam>().WithMany()
                        .HasForeignKey("BuildTeamId")
                        .HasConstraintName("build_team_has_country_ibfk_1"),
                    j =>
                    {
                        j.HasKey("BuildTeamId", "CountryCode")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("build_team_has_country");
                        j.HasIndex(new[] { "CountryCode" }, "country_code");
                        j.IndexerProperty<int>("BuildTeamId")
                            .HasColumnType("int(11)")
                            .HasColumnName("build_team_id");
                        j.IndexerProperty<string>("CountryCode")
                            .HasMaxLength(2)
                            .HasColumnName("country_code");
                    });

            entity.HasMany(d => d.Uus).WithMany(p => p.BuildTeams)
                .UsingEntity<Dictionary<string, object>>(
                    "BuildTeamHasReviewer",
                    r => r.HasOne<Builder>().WithMany()
                        .HasForeignKey("Uuid")
                        .HasConstraintName("build_team_has_reviewer_ibfk_2"),
                    l => l.HasOne<BuildTeam>().WithMany()
                        .HasForeignKey("BuildTeamId")
                        .HasConstraintName("build_team_has_reviewer_ibfk_1"),
                    j =>
                    {
                        j.HasKey("BuildTeamId", "Uuid")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("build_team_has_reviewer");
                        j.HasIndex(new[] { "Uuid" }, "uuid");
                        j.IndexerProperty<int>("BuildTeamId")
                            .HasColumnType("int(11)")
                            .HasColumnName("build_team_id");
                        j.IndexerProperty<string>("Uuid")
                            .HasMaxLength(36)
                            .HasColumnName("uuid");
                    });
        });

        modelBuilder.Entity<Builder>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("builder");

            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.FirstSlot)
                .HasColumnType("int(11)")
                .HasColumnName("first_slot");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PlotType)
                .HasColumnType("int(11)")
                .HasColumnName("plot_type");
            entity.Property(e => e.Score)
                .HasColumnType("int(11)")
                .HasColumnName("score");
            entity.Property(e => e.SecondSlot)
                .HasColumnType("int(11)")
                .HasColumnName("second_slot");
            entity.Property(e => e.ThirdSlot)
                .HasColumnType("int(11)")
                .HasColumnName("third_slot");
        });

        modelBuilder.Entity<BuilderHasPlot>(entity =>
        {
            entity.HasKey(e => new { e.PlotId, e.Uuid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("builder_has_plot");

            entity.HasIndex(e => e.Uuid, "uuid");

            entity.Property(e => e.PlotId)
                .HasColumnType("int(11)")
                .HasColumnName("plot_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.IsOwner)
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_owner");

            entity.HasOne(d => d.Plot).WithMany(p => p.BuilderHasPlots)
                .HasForeignKey(d => d.PlotId)
                .HasConstraintName("builder_has_plot_ibfk_1");

            entity.HasOne(d => d.Uu).WithMany(p => p.BuilderHasPlots)
                .HasForeignKey(d => d.Uuid)
                .HasConstraintName("builder_has_plot_ibfk_2");
        });

        modelBuilder.Entity<CityProject>(entity =>
        {
            entity.HasKey(e => e.CityProjectId).HasName("PRIMARY");

            entity.ToTable("city_project");

            entity.HasIndex(e => e.CountryCode, "country_code");

            entity.HasIndex(e => e.ServerName, "server_name");

            entity.Property(e => e.CityProjectId).HasColumnName("city_project_id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .HasColumnName("country_code");
            entity.Property(e => e.IsVisible)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_visible");
            entity.Property(e => e.ServerName).HasColumnName("server_name");

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.CityProjects)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_project_ibfk_1");

            entity.HasOne(d => d.ServerNameNavigation).WithMany(p => p.CityProjects)
                .HasPrincipalKey(p => p.ServerName)
                .HasForeignKey(d => d.ServerName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_project_ibfk_2");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode).HasName("PRIMARY");

            entity.ToTable("country");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .HasColumnName("country_code");
            entity.Property(e => e.Continent)
                .HasColumnType("enum('EU','AS','AF','OC','SA','NA')")
                .HasColumnName("continent");
            entity.Property(e => e.CustomModelData)
                .HasMaxLength(255)
                .HasColumnName("custom_model_data");
            entity.Property(e => e.Material)
                .HasMaxLength(255)
                .HasColumnName("material");
        });

        modelBuilder.Entity<Plot>(entity =>
        {
            entity.HasKey(e => e.PlotId).HasName("PRIMARY");

            entity.ToTable("plot");

            entity.HasIndex(e => e.CityProjectId, "city_project_id");

            entity.HasIndex(e => e.DifficultyId, "difficulty_id");

            entity.Property(e => e.PlotId)
                .HasColumnType("int(11)")
                .HasColumnName("plot_id");
            entity.Property(e => e.CityProjectId).HasColumnName("city_project_id");
            entity.Property(e => e.CompleteSchematic)
                .HasColumnType("mediumblob")
                .HasColumnName("complete_schematic");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(36)
                .HasColumnName("created_by");
            entity.Property(e => e.DifficultyId).HasColumnName("difficulty_id");
            entity.Property(e => e.InitialSchematic)
                .HasColumnType("mediumblob")
                .HasColumnName("initial_schematic");
            entity.Property(e => e.IsPasted)
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_pasted");
            entity.Property(e => e.LastActivityDate)
                .HasColumnType("datetime")
                .HasColumnName("last_activity_date");
            entity.Property(e => e.McVersion)
                .HasMaxLength(8)
                .HasColumnName("mc_version");
            entity.Property(e => e.OutlineBounds)
                .HasColumnType("text")
                .HasColumnName("outline_bounds");
            entity.Property(e => e.PlotVersion).HasColumnName("plot_version");
            entity.Property(e => e.Score)
                .HasColumnType("int(11)")
                .HasColumnName("score");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'unclaimed'")
                .HasColumnType("enum('unclaimed','unfinished','unreviewed','completed')")
                .HasColumnName("status");

            entity.HasOne(d => d.CityProject).WithMany(p => p.Plots)
                .HasForeignKey(d => d.CityProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plot_ibfk_1");

            entity.HasOne(d => d.Difficulty).WithMany(p => p.Plots)
                .HasForeignKey(d => d.DifficultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plot_ibfk_2");
        });

        modelBuilder.Entity<PlotDifficulty>(entity =>
        {
            entity.HasKey(e => e.DifficultyId).HasName("PRIMARY");

            entity.ToTable("plot_difficulty");

            entity.Property(e => e.DifficultyId).HasColumnName("difficulty_id");
            entity.Property(e => e.Multiplier)
                .HasPrecision(4, 2)
                .HasDefaultValueSql("'1.00'")
                .HasColumnName("multiplier");
            entity.Property(e => e.ScoreRequirement)
                .HasColumnType("int(11)")
                .HasColumnName("score_requirement");
        });

        modelBuilder.Entity<PlotReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("plot_review");

            entity.HasIndex(e => e.PlotId, "plot_id");

            entity.Property(e => e.ReviewId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("review_id");
            entity.Property(e => e.Feedback)
                .HasMaxLength(256)
                .HasColumnName("feedback");
            entity.Property(e => e.PlotId)
                .HasColumnType("int(11)")
                .HasColumnName("plot_id");
            entity.Property(e => e.Rating)
                .HasMaxLength(7)
                .HasColumnName("rating");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("review_date");
            entity.Property(e => e.ReviewedBy)
                .HasMaxLength(36)
                .HasColumnName("reviewed_by");

            entity.HasOne(d => d.Plot).WithMany(p => p.PlotReviews)
                .HasForeignKey(d => d.PlotId)
                .HasConstraintName("plot_review_ibfk_1");

            entity.HasMany(d => d.Uus).WithMany(p => p.Reviews)
                .UsingEntity<Dictionary<string, object>>(
                    "BuilderHasReviewNotification",
                    r => r.HasOne<Builder>().WithMany()
                        .HasForeignKey("Uuid")
                        .HasConstraintName("builder_has_review_notification_ibfk_2"),
                    l => l.HasOne<PlotReview>().WithMany()
                        .HasForeignKey("ReviewId")
                        .HasConstraintName("builder_has_review_notification_ibfk_1"),
                    j =>
                    {
                        j.HasKey("ReviewId", "Uuid")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("builder_has_review_notification");
                        j.HasIndex(new[] { "Uuid" }, "uuid");
                        j.IndexerProperty<int>("ReviewId")
                            .HasColumnType("int(11)")
                            .HasColumnName("review_id");
                        j.IndexerProperty<string>("Uuid")
                            .HasMaxLength(36)
                            .HasColumnName("uuid");
                    });
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => new { e.BuildTeamId, e.ServerName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("server");

            entity.HasIndex(e => e.ServerName, "server_name").IsUnique();

            entity.Property(e => e.BuildTeamId)
                .HasColumnType("int(11)")
                .HasColumnName("build_team_id");
            entity.Property(e => e.ServerName).HasColumnName("server_name");

            entity.HasOne(d => d.BuildTeam).WithMany(p => p.Servers)
                .HasForeignKey(d => d.BuildTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("server_ibfk_1");
        });

        modelBuilder.Entity<SystemInfo>(entity =>
        {
            entity.HasKey(e => e.SystemId).HasName("PRIMARY");

            entity.ToTable("system_info");

            entity.Property(e => e.SystemId)
                .HasColumnType("int(11)")
                .HasColumnName("system_id");
            entity.Property(e => e.CurrentPlotVersion).HasColumnName("current_plot_version");
            entity.Property(e => e.DbVersion).HasColumnName("db_version");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("last_update");
        });

        modelBuilder.Entity<Tutorial>(entity =>
        {
            entity.HasKey(e => new { e.TutorialId, e.Uuid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("tutorial");

            entity.Property(e => e.TutorialId)
                .HasColumnType("int(11)")
                .HasColumnName("tutorial_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.FirstStageStartDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("first_stage_start_date");
            entity.Property(e => e.IsComplete)
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_complete");
            entity.Property(e => e.LastStageCompleteDate)
                .HasColumnType("datetime")
                .HasColumnName("last_stage_complete_date");
            entity.Property(e => e.StageId)
                .HasColumnType("int(11)")
                .HasColumnName("stage_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
