using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSolution.Api.Dtos;
using SampleSolution.Core.Abstractions;
using SampleSolution.Core.Dtos;
using SampleSolution.Domain.Constants;

namespace SampleSolution.Api.Controllers;

[ApiController]
[Authorize(Roles = RolesConstant.Admin)]
[Route("api/contacts")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserContact([FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();
        var result = await _contactService.GetAllContacts(paginationFilter);
        return Ok(ResponseDto<object>.Success(result));
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetAllUserContact([FromQuery] string searchTerm,
        [FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();
        var result = await _contactService.SearchContact(searchTerm, paginationFilter);
        return Ok(ResponseDto<object>.Success(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserContactById([FromRoute] string id)
    {
        var result = await _contactService.GetContactById(id);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact([FromRoute] string id)
    {
        var result = await _contactService.DeleteContact(id);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }
}