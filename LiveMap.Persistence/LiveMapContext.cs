using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            optionsBuilder.UseSqlServer("defaultPlaceHolder",
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

            entityBuilder.Property(poi => poi.Coordinate)
                .HasColumnType("geometry");
        });

        modelBuilder.Entity<SqlMap>(entityBuilder =>
        {
            entityBuilder.Property(e => e.Border)
                .HasColumnType("geometry") 
                .HasConversion(
                    v => v, 
                    v => v 
                );

            entityBuilder.HasMany(map => map.PointOfInterests)
                .WithOne(poi => poi.Map);

            entityBuilder.Property(map => map.Border)
                .HasColumnType("geometry"); // Or "geometry" if needed

            entityBuilder.Property(map => map.Coordinate)
                .HasColumnType("geometry");
        });
        base.OnModelCreating(modelBuilder);
    }
}
