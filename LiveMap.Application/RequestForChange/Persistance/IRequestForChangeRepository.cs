<<<<<<< HEAD
﻿using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.RequestForChange.Persistance;

public interface IRequestForChangeRepository
{
    public Task<Models.RequestForChange?> GetSingle(Guid id);

    public Task<ICollection<Models.RequestForChange>> GetMultiple(int? skip, int? take);

    public Task<Models.RequestForChange> CreateAsync(Models.RequestForChange requestForChange);
=======
namespace LiveMap.Application.RequestForChange.Persistance;
using Domain.Models;

public interface IRequestForChangeRepository
{
    public Task<RequestForChange> CreateAsync(RequestForChange requestForChange);
>>>>>>> 5cacfa20cb2c0d6944e341269a199509cfdcc956
}