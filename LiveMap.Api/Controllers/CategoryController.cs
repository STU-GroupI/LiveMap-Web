using LiveMap.Api.Models.Category;
using LiveMap.Application;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;
using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    /// <summary>
    /// Get a specific category.
    /// </summary>
    /// <param name="name">The name of the specified Category.</param>
    /// <returns>Returns the specified category. </returns>
    /// <response code="200">Successfully get the categories.</response>
    /// <response code="404">Category not found.</response>
    [HttpGet("{name}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string name,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(name);
        GetSingleResponse response = await handler.Handle(request);

            if (response.Category is null)
            {
                return NotFound();
            }

        var category = response.Category;
        return Ok(category);
    }
    
    /// <summary>
    /// Get multiple categories with optional filtering.
    /// </summary>
    /// <param name="name">Optional name filter (contains).</param>
    /// <param name="skip">Items to skip.</param>
    /// <param name="take">Items to take.</param>
    /// <returns>Filtered category list</returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<List<Category>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMultiple(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(skip, take);
        var response = await handler.Handle(request);
        return Ok(response.Categories);
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="category">The given category.</param>
    /// <returns> The RFC with callback URL </returns>
    /// <response code="201">Response with the created </response>
    /// <response code="500">Something went very wrong</response>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, Category)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(
        [FromBody] CreateSingleCategoryWebRequest webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        var category = new Category()
        {
            CategoryName = webRequest.CategoryName,
            IconName = webRequest.IconName
        };

        var request = new CreateSingleRequest(category);
        CreateSingleResponse response = await handler.Handle(request);
        return CreatedAtAction(
            nameof(Get), 
            new { name = response.Category.CategoryName }, 
            response.Category);
    }


    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="category">The given category.</param>
    /// <returns> The RFC with callback URL </returns>
    /// <response code="201">Response with the created </response>
    /// <response code="500">Something went very wrong</response>
    [HttpPut("{oldName}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, Category)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(
        [FromRoute] string oldName,
        [FromBody] UpdateSingleCategoryWebRequest webRequest,
        [FromServices] IRequestHandler<UpdateSingleRequest, UpdateSingleResponse> handler)
    {
        var request = new UpdateSingleRequest(oldName, webRequest.CategoryName, webRequest.IconName);
        var response = await handler.Handle(request);

        if(!response.Success)
        {
            return NotFound("Category not found");
        }

        return Ok(new Category
        {
            CategoryName = response.NewName,
            IconName = response.iconName
        });
    }

    /// <summary>
    /// Deletes a category by name.
    /// </summary>
    /// <param name="name">Name of the category to delete.</param>
    /// <returns>Status of the deletion</returns>
    [HttpDelete("{name}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(
        [FromRoute] string name,
        [FromServices] IRequestHandler<DeleteSingleRequest, DeleteSingleResponse> handler)
    {
        var request = new DeleteSingleRequest(name);

        var response = await handler.Handle(request);
        if (!response.Success)
        {
            return NotFound("Category not found");
        }

        return NoContent();
    }
}