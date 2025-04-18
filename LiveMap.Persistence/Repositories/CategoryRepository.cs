using LiveMap.Application.Category.Persistance;
using LiveMap.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LiveMap.Persistence.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly LiveMapContext _context;

    public CategoryRepository(LiveMapContext context)
    {
        _context = context;
    }

    public async Task<Category[]> GetMultiple(int? skip, int? take)
    {
        var query = _context.Categories.AsQueryable();

        {
            if (skip is int fromValue)
            {
                query = query.Skip(fromValue);
            }
        }

        if (take is int amountValue)
        {
            query = query.Take(amountValue);
        }

        var result = await query.ToListAsync();
        if (result is not { Count: > 0 })
        {
            return [];
        }

        return result.ToArray();
    }

public async Task<Category?> GetSingle(string name)
    {
        return await _context.Categories.FindAsync(name);
    }
}
