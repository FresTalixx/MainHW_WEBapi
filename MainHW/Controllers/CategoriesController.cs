using AutoMapper;
using MainHW.Models;
using MainHW.Models.DTO;
using MainHW.Services;
using Microsoft.AspNetCore.Mvc;

namespace MainHW.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;
    public readonly IMapper _mapper;

    public CategoriesController(CategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult" /> containing the categories.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategories();
        return Ok(categories);
    }

    /// <summary>
    /// Retrieves a paginated list of categories.
    /// </summary>
    /// <param name="request">The pagination parameters.</param>
    /// <returns>An <see cref="OkObjectResult"/> containing the paginated categories.</returns>
    [HttpGet("paginated")]
    public async Task<ActionResult<PaginetadResult<Category>>> GetPaginatedCategories([FromQuery] PageResultRequest<CategoryFilter> request)
    {
        var paginatedResult = await _categoryService.GetPaginatedCategories(request);
        return Ok(paginatedResult);
    }

    /// <summary>
    /// Adds a new category for the authenticated user.
    /// </summary>
    /// <param name="request">The category details.</param>
    /// <returns>An <see cref="OkObjectResult"/> containing the new category</returns>
    [HttpPost]
    public async Task<IActionResult> AddNewCategory(CategoryDto request)
    {
        var newCategory = _mapper.Map<Category>(request);
        await _categoryService.AddNewCategory(newCategory);
        return Ok(newCategory);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The identifier of the category to update.</param>
    /// <param name="request">The updated category data.</param>
    /// <returns>An action result containing the updated category</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryDto request)
    {
        var category = await _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        _mapper.Map(request, category);
        await _categoryService.UpdateCategory(category);
        return Ok(category);
    }

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <returns>a not found response if the category does not exist;
    /// otherwise, a no content response.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }
        await _categoryService.DeleteCategory(id);
        return NoContent();
    }
}
