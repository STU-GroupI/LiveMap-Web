using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers;

public sealed record ExampleGetSingleRequest(int Id);
public sealed record ExampleGetSingleResult(Person Person);

public interface IPersonRepository
{
    Task<Person> Get(int id);
}
public class PersonRepository : IPersonRepository
{
    public Task<Person> Get(int id)
    {
        int age = 33;
        return Task.FromResult(new Person(
            id,
            "Dave",
            "Daverson",
            DateOnly.FromDateTime(DateTime.Now.Date.AddYears(-age))
        ));
    }
}

public interface IRequestHandler<TRequest, TResult>
{
    Task<TResult> Handle(TRequest request);
}
public sealed class ExampleGetSingleRequestHandler : IRequestHandler<ExampleGetSingleRequest, ExampleGetSingleResult>
{
    private readonly IPersonRepository _personRepository;
    public ExampleGetSingleRequestHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    public async Task<ExampleGetSingleResult> Handle(ExampleGetSingleRequest request)
    {
        Person result = await _personRepository.Get(request.Id);
        return new ExampleGetSingleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class CompleteExampleController : ControllerBase
{
    /*
     * For the record, this region is filled with stuff you would not put here.
     * It is just here to provide a concrete example in one spot.
     */

    public sealed record ExampleGetSingleWebRequest([FromRoute(Name = "id")] int Id);
    /// <summary>
    /// An exmaple request for getting some data
    /// </summary>
    /// <param name="request">
    ///     The webrequest send to the controller
    /// </param>
    /// <param name="handler">
    ///     The required requesthandler
    /// </param>
    /// <returns>
    ///     A single person, the request send as a 404 if the content is not found
    /// </returns>
    /// <response code="200">Returns the requested person.</response>
    /// <response code="404">If the person is not found.</response> 
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> ExampleGet(
        [FromRoute] ExampleGetSingleWebRequest request,
        [FromServices] IRequestHandler<ExampleGetSingleRequest, ExampleGetSingleResult> handler)
    {
        // At this point we may assume that the request is valid.
        // FluentValidation will have done its work by now...
        ExampleGetSingleRequest internalRequest = new(request.Id);
        ExampleGetSingleResult result = await handler.Handle(internalRequest);

        // Now one thing here, we are using null to show that something does not exist.
        // This is fine for very simple implementations, but please use the result pattern
        // the second you notice a result can be more then 1 positive or 1 alternative.
        if (result.Person is null)
        {
            return NotFound(request);
        }

        return Ok(result.Person);
    }
}