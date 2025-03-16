using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Controllers;

[ApiController]
[Route("api/person")] // [Route("api/[controller]")] <- Would be what you'd put here
public class FancyExampleController : ControllerBase
{
    public record GetSingleWebRequest([FromRoute(Name = "id")] int Id);

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]GetSingleWebRequest request)
    {
        return Ok(new Person(
            request.Id,
            "SomePerson",
            "SomeLastName",
            DateOnly.FromDateTime(DateTime.Now.Date)
        ));
    }
    public record GetPagedWebRequest([FromQuery(Name = "from")] int From, [FromQuery(Name = "amount")] int Amount);

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromRoute]GetPagedWebRequest request)
    {
        Person[] data = [new Person(
            1,
            "SomePerson",
            "SomeLastName",
            DateOnly.FromDateTime(DateTime.Now.Date)
        ), new Person(
            2,
            "SomeOtherPerson",
            "SomeOtherLastName",
            DateOnly.FromDateTime(DateTime.Now.Date)
        )];
        return Ok(data);
    }

    public record CreateSingleWebRequest([FromBody]Person Data);
    [HttpPost]
    public async Task<IActionResult> Post(CreateSingleWebRequest request)
    {
        var newPerson = new Person(
            1,
            "",
            "",
            DateOnly.FromDateTime(DateTime.Now.Date)
        );
        return Created($"baseurl/api/person/{newPerson.Id}", newPerson);
    }
}

public record Person(int Id, string FirstName, string LastName, DateOnly DateOfBirth);

/*
    Request -> FluentValidation -> Endpoint -> /Logica/ -> StatusCode(Response)
     
    // Zonder Micro services
    Request -> Handler -> Repositories & Services -> Mogelijk database -> Result
    // Met Micro Services
    Command | Query | Request -> MediatR -> Handler -> Repositories & Services -> Mogelijk database -> Result

    https://miro.medium.com/v2/resize:fit:678/1*dyEEkN3GHQeg7sA6v22EHw.png
*/