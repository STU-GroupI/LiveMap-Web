using LiveMap.Domain.Models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.DbModels;

public class SqlPointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Point Coordinate { get; set; }

    public string? CategoryName { get; set; }
    public required Category Category { get; set; }

    public string? StatusName { get; set; }
    public required PointOfInterestStatus Status { get; set; }

    public required Guid MapId { get; set; }
    public required SqlMap Map { get; set; }
}