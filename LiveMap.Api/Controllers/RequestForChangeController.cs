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

    [HttpPatch]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, RequestForChange)>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch(
        [FromBody] UpdateSingleRfcWebRequest webRequest,
        [FromServices] IRequestHandler<UpdateSingleRequest, UpdateSingleResponse> handler
        )
    {
        try
        {
            var rfc = webRequest.RequestForChange;
            rfc.ApprovalStatus = webRequest.ApprovalStatus.ToString();

            if (webRequest.ApprovalStatus.ToString() == ApprovalStatus.APPROVED)
            {
                rfc.ApprovedOn = DateTime.UtcNow;
            }
            var request = new UpdateSingleRequest(rfc);
            UpdateSingleResponse response = await handler.Handle(request);
            return Ok(response.Rfc);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }
}