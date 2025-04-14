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

    public async Task<bool> Delete(Category category)
    {
        // At this point may consider using Dapper instead. Every executeUpdate is a trip to the database.
        // In our case this doesn't matter to much since this isn't a very hot path, but if we are dealing
        // with a hot code path an this much back and forward db calling, consider using ExecuteSqlRawAsync
        // or Dapper instead.

        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _context.PointsOfInterest.ExecuteUpdateAsync(setters =>
            setters.SetProperty(poi => poi.CategoryName, Category.EMPTY));

        await _context.SuggestedPointsOfInterest.ExecuteUpdateAsync(setters =>
            setters.SetProperty(poi => poi.CategoryName, Category.EMPTY));

        _context.Categories.Remove(category);

        try
        {
            // Commit transaction if all commands succeed, transaction will auto-rollback
            // when disposed if either commands fails
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Category[]> GetMultiple(int? skip, int? take)
    {
        var query = _context.Categories.AsQueryable();

        if (skip is int skipValue)
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

