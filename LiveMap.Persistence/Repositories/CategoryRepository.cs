using LiveMap.Application.Category.Persistance;
using LiveMap.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        if(skip is int skipValue)
        {
            query = query.Skip(skipValue);
        }
        if (take is int takeValue)
        {
            query = query.Take(takeValue);
        }

        return await query.ToArrayAsync();
    }

    public async Task<Category?> GetSingle(string name)
    {
        return await _context.Categories.FindAsync(name);
    }
}
