using MainHW.Models;
using MainHW.Models.DTO;
using MainHW.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MainHW.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;
    

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    private static Func<IEnumerable<Category>, IEnumerable<Category>>? GetFilterFunc(CategoryFilter? filter)
    {
        if (filter != null)
        {
            return categories =>
                categories
                    .Where(c => c.Name.Contains(filter.Name ?? string.Empty))
                    .OrderBy(c => c.Name);
        }

        return categories => categories;
    }

    public static Func<IQueryable<Category>, IQueryable<Category>>? GetOrderByFunc(string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "id" => query => query.OrderBy(c => c.Id),
            "-id" => query => query.OrderByDescending(c => c.Id),
            "name" => query => query.OrderBy(c => c.Name),
            "-name" => query => query.OrderByDescending(c => c.Name),
            "createddate" => query => query.OrderBy(c => c.CreationDate),
            "-createddate" => query => query.OrderByDescending(c => c.CreationDate),
            "updateddate" => query => query.OrderBy(c => c.UpdateDate),
            "-updateddate" => query => query.OrderByDescending(c => c.UpdateDate),
            _ => null
        };
    }

    public async Task<ICollection<Category>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories;
    }

    public async Task UpdateBgAsync(string filename, int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            throw new ArgumentException("Category not found.");
        }

        category.BgFileName = filename;
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task<PaginetadResult<Category>> GetPaginatedCategories(PageResultRequest<CategoryFilter> request)
    {
        var paginatedResult = await _categoryRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            GetOrderByFunc(request.OrderBy),
            GetFilterFunc(request.Filter));

        return paginatedResult;
    }

    public async Task<Category> AddNewCategory(Category newCategory)
    {
        _categoryRepository.Add(newCategory);
        await _categoryRepository.SaveChangesAsync();
        return newCategory;
    }

    public async Task<Category?> UpdateCategory(Category updatedCategory)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(updatedCategory.Id);
        if (existingCategory == null)
        {
            return null;
        }
        existingCategory.Name = updatedCategory.Name;
        existingCategory.BgFileName = updatedCategory.BgFileName;
        existingCategory.UpdateDate = DateTime.Now;
        await _categoryRepository.SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            return false;
        }
        _categoryRepository.Remove(existingCategory);
        await _categoryRepository.SaveChangesAsync();
        return true;
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

}
