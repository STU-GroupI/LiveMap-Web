﻿using LiveMap.Application;
using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/poi")]
public class PointOfInterestController : ControllerBase
{
    /// <summary>
    /// Gets the POI's for a specified map.
    /// </summary>
    /// <param name="id">The id of the specified POI.</param>
    /// <returns>Returns the specified poi. </returns>
    /// <response code="200">Successfully get the poi's.</response>
    /// <response code="404">Poi not found.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(Guid.Parse(id));
        GetSingleResponse response = await handler.Handle(request);

        if (response.PointOfInterest is null)
        {
            return NotFound();
        }

        var poi = response.PointOfInterest;
        poi.Map = null!;
        return Ok(poi);
    }
    /// <summary>
    /// Gets the POI's for a specified map.
    /// </summary>
    /// <param name="mapId">The map to get the POI's from.</param>
    /// <param name="skip">The amount of items to skip. The base is 0.</param>
    /// <param name="take">The amount of items to take. The base is all.</param>
    /// <returns>Returns the poi's from the given map.</returns>
    /// <response code="200">Successfully get the poi's.</response>
    /// <response code="404">Map not found.</response>
    [HttpGet("")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultipleForPark(
        [FromQuery] string mapId,
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(Guid.Parse(mapId), skip, take);
        var response = await handler.Handle(request);

        return Ok(response.PointsOfInterests);
    }
}