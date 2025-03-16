using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;
public struct Coordinate
{
    public readonly double Longitude;
    public readonly double Latitude;

    public Coordinate(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}