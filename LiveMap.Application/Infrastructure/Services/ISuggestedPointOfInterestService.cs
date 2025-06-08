using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Infrastructure.Services;

public interface ISuggestedPointOfInterestService
{
    public Task<ExternalHttpResponse<SuggestedPointOfInterest>> Get(Guid id);
}
