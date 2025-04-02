using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.RequestForChange.Persistance;

public interface IRequestForChangeRepository
{
    public Task<Models.RequestForChange?> GetSingle(Guid id);

    public Task<ICollection<Models.RequestForChange>> GetMultiple(int? skip, int? take);
}