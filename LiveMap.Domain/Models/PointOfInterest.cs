using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;
public class PointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Coordinate Coordinate { get; set; }

    public string? CategoryName { get; set; }
    public required Category Category { get; set; }

    public string? StatusName { get; set; }
    public required PointOfInterestStatus Status { get; set; }

    public required Guid MapId { get; set; }
    public required Map Map { get; set; }

    public virtual ICollection<OpeningHours> OpeningHours { get; set; }
}