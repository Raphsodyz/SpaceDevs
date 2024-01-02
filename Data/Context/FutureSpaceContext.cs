using System;
using Data.Materializated.Views;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.Context
{
    public class FutureSpaceContext : DbContext
    {
        public FutureSpaceContext(DbContextOptions<FutureSpaceContext> options):base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        
        public DbSet<Launch> Launch { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<LaunchServiceProvider> LaunchServiceProvider { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Mission> Mission { get; set; }
        public DbSet<Orbit> Orbit { get; set; }
        public DbSet<Pad> Pad { get; set; }
        public DbSet<Rocket> Rocket { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UpdateLog> UpdateLog { get; set; }
        public DbSet<LaunchView> LaunchView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var connection = Environment.GetEnvironmentVariable(configuration.GetSection("ConnectionStrings:default").Value);
                optionsBuilder.UseNpgsql(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LaunchView>(entity =>
            {
                entity.ToView("launch_view");

                entity.Property(e => e.Id).HasColumnName("launch_id");
                entity.Property(e => e.AtualizationDate).HasColumnName("launch_atualization_date");
                entity.Property(e => e.ImportedT).HasColumnName("launch_imported_t");
                entity.Property(e => e.EntityStatus).HasColumnName("launch_status");
                entity.Property(e => e.ApiGuId).HasColumnName("launch_api_guid");
                entity.Property(e => e.Url).HasColumnName("launch_url");
                entity.Property(e => e.LaunchLibraryId).HasColumnName("launch_launch_library_id");
                entity.Property(e => e.Slug).HasColumnName("launch_slug");
                entity.Property(e => e.Name).HasColumnName("launch_name");
                entity.Property(e => e.IdStatus).HasColumnName("id_status");
                entity.Property(e => e.Net).HasColumnName("launch_net");
                entity.Property(e => e.WindowEnd).HasColumnName("launch_window_end");
                entity.Property(e => e.WindowStart).HasColumnName("launch_window_start");
                entity.Property(e => e.Inhold).HasColumnName("launch_inhold");
                entity.Property(e => e.TbdTime).HasColumnName("launch_tbd_time");
                entity.Property(e => e.TbdDate).HasColumnName("launch_tbd_date");
                entity.Property(e => e.Probability).HasColumnName("launch_probability");
                entity.Property(e => e.HoldReason).HasColumnName("launch_hold_reason");
                entity.Property(e => e.FailReason).HasColumnName("launch_fail_reason");
                entity.Property(e => e.Hashtag).HasColumnName("launch_hashtag");
                entity.Property(e => e.IdLaunchServiceProvider).HasColumnName("id_launch_service_provider");
                entity.Property(e => e.IdRocket).HasColumnName("id_rocket");
                entity.Property(e => e.IdMission).HasColumnName("id_mission");
                entity.Property(e => e.IdPad).HasColumnName("id_pad");
                entity.Property(e => e.WebcastLive).HasColumnName("launch_web_cast_live");
                entity.Property(e => e.Image).HasColumnName("launch_image");
                entity.Property(e => e.Infographic).HasColumnName("launch_infographic");
                entity.Property(e => e.Programs).HasColumnName("launch_programs");
                entity.OwnsOne(c => c.Status, st => 
                {
                    st.Property(x => x.Name).HasColumnName("status_name");
                    st.Property(x => x.Abbrev).HasColumnName("status_abbrev");
                    st.Property(x => x.Description).HasColumnName("status_description");
                });
                entity.OwnsOne(c => c.LaunchServiceProvider, lsp => 
                {
                    lsp.Property(x => x.Name).HasColumnName("launch_service_provider_name");
                    lsp.Property(x => x.Url).HasColumnName("launch_service_provider_url");
                    lsp.Property(x => x.Type).HasColumnName("launch_service_provider_type");
                });
                entity.OwnsOne(c => c.Rocket, r => 
                {
                    r.Property(x => x.IdConfiguration).HasColumnName("id_configuration");
                    r.OwnsOne(a => a.Configuration, cfg => 
                    {
                        cfg.Property(x => x.LaunchLibraryId).HasColumnName("configuration_launch_library_id");
                        cfg.Property(x => x.Url).HasColumnName("configuration_url");
                        cfg.Property(x => x.Name).HasColumnName("configuration_name");
                        cfg.Property(x => x.Family).HasColumnName("configuration_family");
                        cfg.Property(x => x.FullName).HasColumnName("configuration_full_name");
                        cfg.Property(x => x.Variant).HasColumnName("configuration_variant");
                    });
                });
                entity.OwnsOne(c => c.Mission, m => 
                {
                    m.Property(x => x.LaunchLibraryId).HasColumnName("mission_launch_library_id");
                    m.Property(x => x.Name).HasColumnName("mission_name");
                    m.Property(x => x.Description).HasColumnName("mission_description");
                    m.Property(x => x.Type).HasColumnName("mission_type");
                    m.Property(x => x.IdOrbit).HasColumnName("id_orbit");
                    m.Property(x => x.LaunchDesignator).HasColumnName("mission_launch_designator");
                    m.OwnsOne(a => a.Orbit, ob => 
                    {
                        ob.Property(x => x.Name).HasColumnName("orbit_name");
                        ob.Property(x => x.Abbrev).HasColumnName("orbit_abbrev");
                    });
                });
                entity.OwnsOne(c => c.Pad, p => 
                {
                    p.Property(x => x.Url).HasColumnName("pad_url");
                    p.Property(x => x.AgencyId).HasColumnName("pad_agency_id");
                    p.Property(x => x.Name).HasColumnName("pad_name");
                    p.Property(x => x.InfoUrl).HasColumnName("pad_info_url");
                    p.Property(x => x.WikiUrl).HasColumnName("pad_wiki_url");
                    p.Property(x => x.MapUrl).HasColumnName("pad_map_url");
                    p.Property(x => x.Latitude).HasColumnName("pad_latitude");
                    p.Property(x => x.Longitude).HasColumnName("pad_longitude");
                    p.Property(x => x.IdLocation).HasColumnName("id_location");
                    p.Property(x => x.MapImage).HasColumnName("pad_map_image");
                    p.Property(x => x.TotalLaunchCount).HasColumnName("pad_total_launch_count");
                    p.OwnsOne(a => a.Location, loc => 
                    {
                        loc.Property(x => x.Url).HasColumnName("location_url");
                        loc.Property(x => x.Name).HasColumnName("location_name");
                        loc.Property(x => x.CountryCode).HasColumnName("location_country_code");
                        loc.Property(x => x.MapImage).HasColumnName("location_map_image");
                        loc.Property(x => x.TotalLaunchCount).HasColumnName("location_total_launch_count");
                        loc.Property(x => x.TotalLandingCount).HasColumnName("location_total_landing_count");
                    });
                });
            });

            modelBuilder.Entity<Configuration>()
                .Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name || ' ' || family)");

            modelBuilder.Entity<Mission>()
                .Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name)");

            modelBuilder.Entity<Location>()
                .Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name)");

            modelBuilder.Entity<Pad>()
                .Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name)");

            modelBuilder.Entity<Launch>()
                .Property(e => e.Search)
                .HasComputedColumnSql("LOWER(name || ' ' || slug)");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.BeforeSave();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.BeforeSave();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
