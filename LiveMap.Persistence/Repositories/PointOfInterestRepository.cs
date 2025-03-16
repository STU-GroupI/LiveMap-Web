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
            new PointOfInterest()
            {
                Id = 1,
                Title = "Toilet",
                Description = "TestToilet 1"
            },
            new PointOfInterest()
            {
                Id = 2,
                Title = "Toilet",
                Description = "TestToilet 2"
            },
            new PointOfInterest()
            {
                Id = 3,
                Title = "Toilet",
                Description = "TestToilet 3"
            },
            new PointOfInterest()
            {
                Id = 4,
                Title = "Toilet",
                Description = "TestToilet 4"
            },
            new PointOfInterest()
            {
                Id = 5,
                Title = "Toilet",
                Description = "TestToilet 5"
            },
            new PointOfInterest()
            {
                Id = 6,
                Title = "Toilet",
                Description = "TestToilet 6"
            },
            new PointOfInterest()
            {
                Id = 7,
                Title = "Toilet",
                Description = "TestToilet 7"
            },
            new PointOfInterest()
            {
                Id = 8,
                Title = "Toilet",
                Description = "TestToilet 8"
            }
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