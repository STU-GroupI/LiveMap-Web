using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using LiveMapDashboard.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace LiveMapDashboard.Web.Controllers
{
    [ApiController]
    [Route("api/poi")]
    public class PointOfInterestController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
            [FromRoute] GetSingleRequest request,
            [FromServices] GetSingleHandler handler)
        {
            // Can be moved ones FluentValidation is in place
            if(request is not { Id: > 0})
            {
                return BadRequest();
            }

            GetSingleResponse response = await handler.Handle(request);

            if(response.PointOfInterest is null)
            {
                return NoContent();
            }

            var poi = response.PointOfInterest;

            return Ok($"{poi.Title} {poi.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery]int parkId, [FromQuery]int from, [FromQuery]int? amount)
        {
            return Ok("Test");
        }
    }
}