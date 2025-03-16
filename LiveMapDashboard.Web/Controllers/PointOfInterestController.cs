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
        public async Task<IActionResult> Get(GetSingleRequest request)
        {
            GetSingleHandler handler = new GetSingleHandler(new PointOfInterestRepository());

            GetSingleResponse response = handler.GetFromRepo(request);

            return Ok(response.PointOfInterest.Title);
        }
    }
}