using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;

namespace LiveMap.Persistence.Repositories;

public class PointOfInterestRepository : IPointOfInterestRepository
{
    private List<PointOfInterest> pointsOfInterestList;

    // Temporary constructor for testing
    public PointOfInterestRepository()
    {
        pointsOfInterestList = new List<PointOfInterest>
        {
            new PointOfInterest(1, "Toilet", "TestToilet 1"),
            new PointOfInterest(2, "Point of Interest 2", "Description 2"),
            new PointOfInterest(3, "Point of Interest 3", "Description 3"),
            new PointOfInterest(4, "Point of Interest 4", "Description 4"),
            new PointOfInterest(5, "Point of Interest 5", "Description 5"),
            new PointOfInterest(6, "Point of Interest 6", "Description 6"),
            new PointOfInterest(7, "Point of Interest 7", "Description 7"),
            new PointOfInterest(8, "Point of Interest 8", "Description 8"),
            new PointOfInterest(9, "Point of Interest 9", "Description 9"),
            new PointOfInterest(10, "Point of Interest 10", "Description 10"),
        };
    }

    public Task<PointOfInterest> GetPaged(int from, int amount)
    {
        throw new NotImplementedException();
    }

    public Task<PointOfInterest> GetSingle(int id)
    {
        if (pointsOfInterestList == null)
        {
            return Task.FromException<PointOfInterest>(new ArgumentNullException(nameof(pointsOfInterestList)));
        }

        PointOfInterest pointOfInterest = pointsOfInterestList.FirstOrDefault(p => p.Id == id);

        if (pointOfInterest == null)
        {
            return Task.FromException<PointOfInterest>(new ArgumentNullException(nameof(id)));
        }

        return Task.FromResult(pointOfInterest);
    }
}