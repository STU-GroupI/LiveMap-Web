using LiveMap.Application.Category.Persistance;
using LiveMap.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public async Task<Category> Create(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    /*IMPORTANT NOTE:
        TODO: This Update method is merely a workaround for the fact that we cannot update a category name in the database,
        this is because of a foreign key constraint that prevents us from updating a category name in the database whatsoever.
        In this workaround, a new category is created and the old category is deleted. This is not ideal, but it works for now.
        Maybe use ids for the future...?
    */
    public async Task<bool> Update(string oldName, string newName, string iconName)
    {
        // Start the transaction
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(iconName))
            {
                return false;
            }

            // Find the category entity by old name
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == oldName);

            if (category is null)
            {
                // Rollback if the category doesn't exist
                await transaction.RollbackAsync();
                return false;
            }
            
            if (oldName == newName)
            {
                // Just update icon name
                category.IconName = iconName;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            
            //Create a new category entity with the new name
            Category newCategory = new Category
            {
                CategoryName = newName,
                IconName = iconName,
            };

            //Add the entity to the context
            _context.Categories.Add(newCategory);

            await _context.SaveChangesAsync();

            // Update PointsOfInterest and SuggestedPointsOfInterest records first
            await _context.PointsOfInterest
                .Where(poi => poi.CategoryName == oldName)
                .ExecuteUpdateAsync(setters => setters.SetProperty(poi => poi.CategoryName, newName));

            await _context.SuggestedPointsOfInterest
                .Where(poi => poi.CategoryName == oldName)
                .ExecuteUpdateAsync(setters => setters.SetProperty(poi => poi.CategoryName, newName));

            //Remove the old category
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            // Rollback if something goes wrong
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> Delete(string name)
    {
        var category = await _context.Categories
            .Where(c => c.CategoryName == name)
            .FirstOrDefaultAsync();

        if (category is null) return false;

        // We need to set the CategoryName to EMPTY for all PointsOfInterest and SuggestedPointsOfInterest
        // before we can delete the category. This is because of the foreign key constraint in the database.
        // We could also use a cascade delete, but this would delete all PointsOfInterest and SuggestedPointsOfInterest
        // that are related to this category. We don't want that, so we set the CategoryName to EMPTY instead.
        // At this point may consider using Dapper instead. Every executeUpdate is a trip to the database.
        // In our case this doesn't matter to much since this isn't a very hot path, but if we are dealing
        // with a hot code path an this much back and forward db calling, consider using ExecuteSqlRawAsync
        // or Dapper instead.

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.PointsOfInterest.Where(poi => poi.CategoryName == name).ExecuteUpdateAsync(setters =>
            setters.SetProperty(poi => poi.CategoryName, Category.EMPTY));

            await _context.SuggestedPointsOfInterest.Where(poi => poi.CategoryName == name).ExecuteUpdateAsync(setters =>
            setters.SetProperty(poi => poi.CategoryName, Category.EMPTY));

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            // Commit transaction if all commands succeed, transaction will auto-rollback
            // when disposed if either commands fails
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
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
        query = query.Where(value => value.CategoryName != Category.EMPTY);

        var categories = await query.ToArrayAsync();
        foreach (var category in categories)
        {
            ComputeCategoryInUse(category);
        }
        
        return categories;
    }

    public async Task<Category?> GetSingle(string name)
    {
        if(name == Category.EMPTY)
        {
            return null;
        }

        var category = await _context.Categories.FindAsync(name);
        if (category is not null)
        {
            ComputeCategoryInUse(category);
        }
        
        return await _context.Categories.FindAsync(name);
    }
    
    private void ComputeCategoryInUse(Category category)
    {
        category.InUse = _context.PointsOfInterest.Any(poi => poi.CategoryName == category.CategoryName);
    }
}

