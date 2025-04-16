using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Category.Requests;
public sealed record GetMultipleRequest(string name, int? Skip, int? Take);