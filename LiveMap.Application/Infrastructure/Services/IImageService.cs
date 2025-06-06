using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Infrastructure.Services;

public interface IImageService
{
    Task<ExternalHttpResponse<string>> Create(Image image); // returns url to image
}
