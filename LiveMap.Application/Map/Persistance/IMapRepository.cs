using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Map.Persistance;
public interface IMapRepository
{
    public Task<Models.Map?> GetSingle(Guid id);

    public Task<ICollection<Models.Map>> GetMultiple(int? skip, int? take);

    public Task<bool> UpdateMapBorder(Guid id, Models.Coordinate[] coords);
}

