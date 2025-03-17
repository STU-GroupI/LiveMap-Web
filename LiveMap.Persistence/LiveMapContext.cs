using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LiveMap.Persistence;
public class LiveMapContext : DbContext
{
    public DbSet<SqlPointOfInterest> PointsOfInterest { get; set; }
    public DbSet<PointOfInterestStatus> PoIStatusses { get; set; }
    public DbSet<SqlMap> Maps { get; set; }
    public DbSet<Category> Categories { get; set; }

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
        modelBuilder.Entity<SqlPointOfInterest>(entityBuilder =>
        {
            entityBuilder.ToTable("PointOfInterest")
                .HasOne(poi => poi.Map)
                .WithMany(map => map.PointOfInterests);

            entityBuilder.HasOne(poi => poi.Category)
                .WithMany()
                .HasForeignKey(poi => poi.CategoryName);

            entityBuilder.HasOne(poi => poi.Status)
                .WithMany()
                .HasForeignKey(poi => poi.StatusName);

            entityBuilder.Property(e => e.Position).HasColumnType("geometry");
        });

        modelBuilder.Entity<SqlMap>(entity =>
        {
            entity.Property(e => e.Border).HasColumnType("geometry");
            entity.Property(e => e.Position).HasColumnType("geometry");
        });

        modelBuilder.Entity<Category>(entityBuilder =>
        {
            entityBuilder.HasKey(cat => cat.Name);
        });
        modelBuilder.Entity<PointOfInterestStatus>(entityBuilder =>
        {
            entityBuilder.HasKey(status => status.Status);
        });

        base.OnModelCreating(modelBuilder);
    }
}