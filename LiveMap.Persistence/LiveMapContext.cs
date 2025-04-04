using LiveMap.Domain.Models;
using LiveMap.Persistence.DataSeeder;
using LiveMap.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LiveMap.Persistence;
public class LiveMapContext : DbContext
{
    public DbSet<SqlPointOfInterest> PointsOfInterest { get; set; }
    public DbSet<SqlMap> Maps { get; set; }
    public DbSet<SqlSuggestedPointOfInterest> SuggestedPointsOfInterest { get; set; }
    public DbSet<SqlRequestForChange> RequestsForChange { get; set; }

    public DbSet<PointOfInterestStatus> PoIStatusses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
    public DbSet<SqlOpeningHours> OpeningHours { get; set; }

    public LiveMapContext(DbContextOptions<LiveMapContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("PlaceHolder",
                sqlOptions => sqlOptions.UseNetTopologySuite());
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<SqlPointOfInterest>(entityBuilder =>
        {
            entityBuilder.ToTable("PointOfInterest")
                .HasKey(e => e.Id);

            entityBuilder.HasOne(poi => poi.Map)
                .WithMany(map => map.PointOfInterests);

            entityBuilder.HasOne(poi => poi.Category)
                .WithMany()
                .HasForeignKey(poi => poi.CategoryName);

            entityBuilder.HasOne(poi => poi.Status)
                .WithMany()
                .HasForeignKey(poi => poi.StatusName);

            entityBuilder.HasMany(poi => poi.OpeningHours)
                .WithOne(oh => oh.Poi)
                .HasForeignKey(oh => oh.PoiId);

            entityBuilder.Property(e => e.Position).HasColumnType("geometry");
        });

        modelBuilder.Entity<SqlMap>(entityBuilder =>
        {
            entityBuilder.ToTable("Map")
                .HasKey(e => e.Id);
            entityBuilder.Property(e => e.Border).HasColumnType("geometry");
            entityBuilder.Property(e => e.Position).HasColumnType("geometry");
        });

        modelBuilder.Entity<Category>(entityBuilder =>
        {
            entityBuilder.ToTable("Category")
                .HasKey(cat => cat.CategoryName);
            entityBuilder.Property(cat => cat.CategoryName)
                .HasColumnName("Category");
        });

        modelBuilder.Entity<PointOfInterestStatus>(entityBuilder =>
        {
            entityBuilder.ToTable("Status")
                .HasKey(status => status.Status);
        });

        modelBuilder.Entity<ApprovalStatus>(entityBuilder =>
        {
            entityBuilder.HasKey(status => status.Status);
        });

        modelBuilder.Entity<SqlSuggestedPointOfInterest>(entityBuilder =>
        {
            entityBuilder.ToTable("SuggestedPointOfInterest")
                .HasKey(e => e.Id);

            entityBuilder.HasOne(poi => poi.Map);

            entityBuilder.HasOne(poi => poi.Category)
                .WithMany()
                .HasForeignKey(poi => poi.CategoryName);

            entityBuilder.HasOne(poi => poi.Status)
                .WithMany()
                .HasForeignKey(poi => poi.StatusName);

            entityBuilder.Property(e => e.Position).HasColumnType("geometry");

            entityBuilder.HasOne(e => e.RFC)
                .WithOne(e => e.SuggestedPoi)
                .HasForeignKey<SqlSuggestedPointOfInterest>(e => new { e.RFCId });
        });

        modelBuilder.Entity<SqlRequestForChange>(entityBuilder =>
        {
            entityBuilder.ToTable("RequestForChange")
                .HasKey(e => e.Id);

            entityBuilder.HasOne(e => e.Poi);

            entityBuilder.HasOne(e => e.SuggestedPoi)
                .WithOne(e => e.RFC)
                .HasForeignKey<SqlRequestForChange>(e => new { e.SuggestedPoiId });

            entityBuilder.HasOne(e => e.StatusProp)
                .WithMany()
                .HasForeignKey(e => new { e.ApprovalStatus });
        });

        // OpeningHours
        modelBuilder.Entity<SqlOpeningHours>(entityBuilder =>
        {
            entityBuilder.ToTable("OpeningHours")
                .HasKey(oh => oh.Id);

            entityBuilder       
                .HasOne(o => o.Poi)
                .WithMany(p => p.OpeningHours)
                .HasForeignKey(o => o.PoiId);
        });
    }
}