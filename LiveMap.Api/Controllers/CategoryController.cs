using LiveMap.Application;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;
using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMap.Api.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        /// <summary>
        /// Gets the specified category.
        /// </summary>
        /// <param name="name">The category name</param>
        /// <returns>Returns the specified category. </returns>
        /// <response code="200">Successfully get the category.</response>
        /// <response code="404">Category not found.</response>
        [HttpGet("{name}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Category>(StatusCodes.Status200OK)]
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

            return Ok(response.Category);
        }

        /// <summary>
        /// Gets the categories
        /// </summary>
        /// <param name="skip">The amount of items to skip. The base is 0.</param>
        /// <param name="take">The amount of items to take. The base is all.</param>
        /// <returns>Returns the categories.</returns>
        /// <response code="200">Successfully gets the categories.</response>
        [HttpGet("")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Category[]>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMultiple(
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
        {
            var request = new GetMultipleRequest(skip, take);
            var response = await handler.Handle(request);

            return Ok(response.Categories);
        }
    }
}
