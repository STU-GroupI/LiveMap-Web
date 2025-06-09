using LiveMap.Api.Models;
using LiveMap.Api.Models.RequestForChange;
using LiveMap.Application;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMap.Application.RequestForChange.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/rfc")]
public class RequestForChangeController : ControllerBase
{
    /// <summary>
    /// Gets the Suggested POI
    /// </summary>
    /// <param name="id">RFC ID</param>
    /// <returns>The Suggested RFC</returns>
    /// <response code="200">Successfully get the RFC's.</response>
    /// <response code="404">Resource not found</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<RequestForChange>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(Guid.Parse(id));
        var response = await handler.Handle(request);

        if(response.RequestForChange is null)
        {
            return NotFound("Request for change not found");
        }

        return Ok(response.RequestForChange);
    }

    /// <summary>
    /// Gets all Suggested POI's for a map
    /// </summary>
    /// <param name="mapId">Map ID</param>
    /// <returns>The Suggested POI's</returns>
    /// <response code="200">Successfully get the suggested poi's.</response>
    /// <response code="404">Map not found.</response>
    [HttpGet("")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PaginatedResult<RequestForChange>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultiple(
        [FromQuery] string mapId,
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromQuery] bool? ascending,
        [FromQuery] bool? isPending,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(Guid.Parse(mapId), skip, take, ascending, isPending);
        var response = await handler.Handle(request);

        return Ok(response.Rfc);
    }

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
        var validationResults = new CreateSingleValidator().Validate(request);

        if (!validationResults.IsValid)
        {
            var errorMessages = string.Join(" ", validationResults.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errorMessages);
        }

        try
        {
            CreateSingleResponse response = await handler.Handle(request);
            return CreatedAtAction(nameof(Get), new { id = response.Rfc.Id.ToString() }, response.Rfc);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "Something went wrong...");
        }
    }

    /// <summary>
    /// Updates an RFC with the given request data
    /// </summary>
    /// <param name="request">The RFC update data</param>
    /// <returns>The updated RFC</returns>
    /// <response code="200">RFC successfully updated</response>
    /// <response code="404">RFC not found</response>
    /// <response code="500">Something went very wrong</response>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, RequestForChange)>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(
        [FromBody] UpdateSingleRfcWebRequest webRequest,
        [FromRoute] Guid id,
        [FromServices] IRequestHandler<UpdateSingleRequest, UpdateSingleResponse> handler
)
    {
        var rfc = new RequestForChange()
        {
            Id = id,
            ApprovalStatus = webRequest.ApprovalStatus,
            ApprovedOn = webRequest.ApprovalStatus == ApprovalStatus.APPROVED ? DateTime.UtcNow : null,
            SubmittedOn = DateTime.Now
        };

        var request = new UpdateSingleRequest(rfc);

        UpdateSingleResponse response = await handler.Handle(request);
        if (response.Rfc is null)
        {
            return NotFound("Request for change not found");
        }

        return Ok(response.Rfc);
    }


    [HttpPost("{id}/approve")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Approve(
        [FromBody] ApproveRfcWebRequest webRequest,
        [FromServices] IRequestHandler<ApprovalRequest, ApprovalResponse> handler)
    {
        var request = new ApprovalRequest(webRequest.Rfc, webRequest.Poi);
        var validationResults = new ApprovalValidator().Validate(request);

        if (!validationResults.IsValid)
        {
            var errorMessages = string.Join(" ", validationResults.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errorMessages);
        }

        try
        {
            ApprovalResponse response = await handler.Handle(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

    [HttpPost("{id}/reject")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Reject(
        string id,
        [FromServices] IRequestHandler<RejectRfcRequest, RejectRfcResponse> handler)
    {
        var request = new RejectRfcRequest(Guid.Parse(id));

        RejectRfcResponse response = await handler.Handle(request);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok();
    }
}