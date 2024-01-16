using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleSolution.Api.Dtos;
using SampleSolution.Core.Abstractions;
using SampleSolution.Core.Dtos;
using SampleSolution.Domain.Entities;

namespace SampleSolution.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users/contacts")]
public class UserContactController : ControllerBase
{
    private readonly IUserContactService _userContactService;
    private readonly UserManager<User> _userManager;

    public UserContactController(IUserContactService userContactService, UserManager<User> userManager)
    {
        _userContactService = userContactService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserContact([FromQuery] PaginationFilter? paginationFilter = null)
    {
        var userId = GetUserId();
        paginationFilter ??= new PaginationFilter();
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userContactService.GetAllUserContacts(userId, paginationFilter);
        return Ok(ResponseDto<object>.Success(result));
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetAllUserContact([FromQuery] string searchTerm,
        [FromQuery] PaginationFilter? paginationFilter = null)
    {
        var userId = GetUserId();
        paginationFilter ??= new PaginationFilter();
        var result = await _userContactService.SearchUserContact(userId, searchTerm, paginationFilter);
        return Ok(ResponseDto<object>.Success(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserContactById([FromRoute] string id)
    {
        var userId = GetUserId();
        var result = await _userContactService.GetUserContactById(userId, id);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactDto contactDto)
    {
        var userId = GetUserId();
        await _userContactService.CreateContact(userId, contactDto);
        return Ok(ResponseDto<object>.Success());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSingleContact([FromRoute] string id,
        [FromBody] UpdateContactDto updatedContactDto)
    {
        var userId = GetUserId();
        var result = await _userContactService.UpdateContact(userId, id, updatedContactDto);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact([FromRoute] string id)
    {
        var userId = GetUserId();
        var result = await _userContactService.DeleteContact(userId, id);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    private string GetUserId()
    {
        return _userManager.GetUserId(User)!;
    }
}