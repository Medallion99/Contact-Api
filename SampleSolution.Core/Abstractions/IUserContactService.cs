using SampleSolution.Core.Dtos;

namespace SampleSolution.Core.Abstractions;

public interface IUserContactService
{
    public Task<PaginatorDto<IEnumerable<GetAllContactDto>>> GetAllUserContacts(string userId,
        PaginationFilter paginationFilter);

    public Task<PaginatorDto<IEnumerable<GetAllContactDto>>> SearchUserContact(string userId, string searchTerm,
        PaginationFilter paginationFilter);

    public Task<Result<SingleContactDto>> GetUserContactById(string userId, string contactId);

    public Task CreateContact(string userId, CreateContactDto contactDto);

    public Task<Result<SingleContactDto>> UpdateContact(string userId, string contactId,
        UpdateContactDto contactDto);

    public Task<Result> DeleteContact(string userId, string contactId);
}