using SampleSolution.Core.Dtos;

namespace SampleSolution.Core.Abstractions;

public interface IContactService
{
    public Task<PaginatorDto<IEnumerable<GetAllContactDto>>> GetAllContacts(PaginationFilter paginationFilter);

    public Task<PaginatorDto<IEnumerable<GetAllContactDto>>> SearchContact(string searchTerm,
        PaginationFilter paginationFilter);

    public Task<Result<SingleContactDto>> GetContactById(string contactId);

    public Task<Result> DeleteContact(string contactId);
}