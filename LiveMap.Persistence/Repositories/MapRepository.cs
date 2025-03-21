﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveMap.Application.Map.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LiveMap.Persistence.Repositories;

public class MapRepository : IMapRepository
{
    private readonly LiveMapContext _context;

    public MapRepository(LiveMapContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Map>> GetMultiple(int? skip, int? take)
    {
        var query = _context.Maps.AsQueryable();

        if (skip is int fromValue)
        {
            query = query.Skip(fromValue);
        }

        if(take is int amountValue)
        {
            query = query.Take(amountValue);
        }
        
        var result = await query.ToListAsync();

        if (result is not { Count: > 0 }) 
        {
            return [];
        }

        return result.Select(map => map.ToMap())
            .ToList();
    }

    public async Task<Map?> GetSingle(Guid id)
    {
        SqlMap? map = await _context.Maps
            .Include(map => map.PointOfInterests)
            .Where(map => map.Id == id)
            .FirstOrDefaultAsync();

        if(map is null)
        {
            return null;
        }


        return map.ToMap();
    }
}