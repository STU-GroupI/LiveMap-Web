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
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
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
        try
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

            return Ok(response.Rfc);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }
}