using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.DbModels;
public class SqlOpeningHours
{
    public required Guid Id { get; set; }

    public required DayOfWeek DayOfWeek { get; set; }

    public required TimeSpan Start { get; set; }
    public required TimeSpan End { get; set; }

    public required Guid PoiId { get; set; }
    public required SqlPointOfInterest Poi { get; set; }
}
