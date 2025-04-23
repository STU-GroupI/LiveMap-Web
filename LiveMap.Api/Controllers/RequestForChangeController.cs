using Microsoft.AspNetCore.Mvc;
using LiveMap.Domain.Models;
using System.Net.Mime;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Api.Models;
using LiveMap.Application;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/rfc")]
public class RequestForChangeController : ControllerBase
{

    /// <summary>
    /// Creates an RFC for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> The RFC with callback URL </returns>
    /// <response code="201">Response with the created </response>
    /// <response code="500">Something went very wrong</response>
    [HttpPost()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, RequestForChange)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(
        [FromBody] CreateSingleRfcWebRequest webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        var rfc = new RequestForChange()
        {
            Id = Guid.Empty,
            Message = webRequest.Message,
            PoiId = webRequest.PoiId,
            ApprovalStatus = string.Empty,
            SubmittedOn = default
        };

        var request = new CreateSingleRequest(rfc);

        try
        {
            CreateSingleResponse response = await handler.Handle(request);
            return Created("", response.Rfc);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

    /// <summary>
    /// Gets all Suggested POI's for a map
    /// </summary>
    /// <param name="id">Map ID</param>
    /// <returns>The Suggested POI's</returns>
    /// <response code="200">Successfully get the suggested poi's.</response>
    /// <response code="404">Map not found.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromQuery] bool? ascending,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(Guid.Parse(id), skip, take, ascending);
        var response = await handler.Handle(request);

        return Ok(response.Rfc);
    }
}