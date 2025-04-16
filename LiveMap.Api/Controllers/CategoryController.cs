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
    /// <param name="name">The id of the specified Category.</param>
    /// <returns>Returns the specified poi. </returns>
    /// <response code="200">Successfully get the poi's.</response>
    /// <response code="404">Poi not found.</response>
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
    /// Creates a new category
    /// </summary>
    /// <param name="category">The given category.</param>
    /// <returns> The RFC with callback URL </returns>
    /// <response code="201">Response with the created </response>
    /// <response code="500">Something went very wrong</response>
    [HttpPost()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, Category)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(
        [FromBody] CreateSingleCategoryWebRequest webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        var poi = new Category()
        {
            CategoryName = webRequest.CategoryName,
        };

        var request = new CreateSingleRequest(poi);

        try
        {
            CreateSingleResponse response = await handler.Handle(request);
            return Created("", response.Category);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
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
        [FromQuery] string? name,
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(name, skip, take);
        var response = await handler.Handle(request);
        return Ok(response.Categories);
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

        try
        {
            var response = await handler.Handle(request);

            if (!response.Success)
                return NotFound("Category not found");

            return NoContent();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

}