using Microsoft.EntityFrameworkCore;
using SampleSolution.Core.Abstractions;
using SampleSolution.Core.Dtos;
using SampleSolution.Core.Utilities;
using SampleSolution.Domain.Entities;

namespace SampleSolution.Core.Services;

public class ContactService : IContactService
{
    private readonly IRepository<Contact> _contactRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ContactService(IRepository<Contact> contactRepository, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatorDto<IEnumerable<GetAllContactDto>>> GetAllContacts(PaginationFilter paginationFilter)
    {
        var contacts = await _contactRepository.GetAll()
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new GetAllContactDto(c.Id, c.Name, c.PhoneNumber))
            .Paginate(paginationFilter);

        return contacts;
    }

    public async Task<PaginatorDto<IEnumerable<GetAllContactDto>>> SearchContact(string searchTerm,
        PaginationFilter paginationFilter)
    {
        var contacts = await _contactRepository.GetAll()
            .Where(c => c.Name.Contains(searchTerm) || c.PhoneNumber.Contains(searchTerm) ||
                        c.EmailAddress.Contains(searchTerm))
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new GetAllContactDto(c.Id, c.Name, c.PhoneNumber))
            .Paginate(paginationFilter);

        return contacts;
    }

    public async Task<Result<SingleContactDto>> GetContactById(string contactId)
    {
        var contact = await _contactRepository.GetAll()
            .Include(c => c.Address)
            .Select(c => new SingleContactDto(
                c.Name,
                c.PhoneNumber,
                $"{c.Address.Number} {c.Address.Street}, {c.Address.City}, {c.Address.State}, {c.Address.Country}"))
            .FirstOrDefaultAsync();

        if (contact is null)
            return new Error[] { new("Contact.Error", "Contact not found") };

        return contact;
    }

    public async Task<Result> DeleteContact(string contactId)
    {
        var contact = await _contactRepository.FindById(contactId);

        if (contact is null)
            return new Error[] { new("Contact.Error", "Contact not found") };

        _contactRepository.Remove(contact);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}