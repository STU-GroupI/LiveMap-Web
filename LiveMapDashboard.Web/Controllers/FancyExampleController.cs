using LiveMapDashboard.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Controllers
{
    public record Person(int Id, string FirstName, string LastName, DateOnly DateOfBirth);

    [ApiController]
    [Route("api/person")] // [Route("api/[controller]")] <- Would be what you'd put here
    public class FancyExampleController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(GetSingleRequest request)
        {
            return Ok(new GetSingleResponse(new Person(
                request.Id,
                "",
                "",
                DateOnly.FromDateTime(DateTime.Now.Date)
            )));
        }
        [HttpGet]
        public async Task<IActionResult> GetPaged(GetSingleRequest request)
        {
            return Ok(new GetSingleResponse(new Person(
                request.Id,
                "",
                "",
                DateOnly.FromDateTime(DateTime.Now.Date)
            )));
        }
    }
    public record GetSingleRequest([FromRoute] int Id);
    public record GetSingleResponse(Person Data);

    public record GetPagedRequest([FromQuery] int From, [FromQuery] int Amount);
    public record GetPagedResponse(Person[] Data);

    public record CreateSingleRequest(string FirstName, string LastName, DateOnly DateOfBirth);
    public record CreateSingleResponse(Person Person);
}

/*
    Request -> FluentValidation -> Endpoint -> /Logica/ -> StatusCode(Response)
     
    // Zonder Micro services
    Request -> Handler -> Repositories & Services -> Mogelijk database -> Result
    // Met Micro Services
    Command | Query | Request -> MediatR -> Handler -> Repositories & Services -> Mogelijk database -> Result

    https://miro.medium.com/v2/resize:fit:678/1*dyEEkN3GHQeg7sA6v22EHw.png
*/