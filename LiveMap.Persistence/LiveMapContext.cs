using LiveMap.Domain.Models;
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
    public DbSet<PointOfInterest> PointsOfInterest { get; set; }
    public DbSet<PointOfInterestStatus> PoIStatusses { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<Category> Categories { get; set; }

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
        modelBuilder.Entity<PointOfInterest>(entity =>
        {
            entity.Property(p => p.Coordinate)
                .HasColumnType("geography")
                .HasConversion(
                    v => new Point(v.Longitude, v.Latitude) { SRID = 4326 },
                    v => v != null ? new Domain.Models.Coordinate(v.Y, v.X) : new(0, 0)
                );
        });
        base.OnModelCreating(modelBuilder);
    }
}
