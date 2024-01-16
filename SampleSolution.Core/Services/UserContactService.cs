using Microsoft.EntityFrameworkCore;
using SampleSolution.Core.Abstractions;
using SampleSolution.Core.Dtos;
using SampleSolution.Core.Utilities;
using SampleSolution.Domain.Entities;

namespace SampleSolution.Core.Services;

public class UserContactService : IUserContactService
{
    private readonly IRepository<Address> _addressRepository;
    private readonly IRepository<Contact> _contactRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserContactService(IRepository<Contact> contactRepository, IRepository<Address> addressRepository,
        IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatorDto<IEnumerable<GetAllContactDto>>> GetAllUserContacts(string userId,
        PaginationFilter paginationFilter)
    {
        var contacts = await _contactRepository.GetAll()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new GetAllContactDto(c.Id, c.Name, c.PhoneNumber))
            .Paginate(paginationFilter);

        return contacts;
    }

    public async Task<PaginatorDto<IEnumerable<GetAllContactDto>>> SearchUserContact(string userId, string searchTerm,
        PaginationFilter paginationFilter)
    {
        var contacts = await _contactRepository.GetAll()
            .Where(c => c.UserId == userId)
            .Where(c => c.Name.Contains(searchTerm) || c.PhoneNumber.Contains(searchTerm) ||
                        c.EmailAddress.Contains(searchTerm))
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new GetAllContactDto(c.Id, c.Name, c.PhoneNumber))
            .Paginate(paginationFilter);

        return contacts;
    }

    public async Task<Result<SingleContactDto>> GetUserContactById(string userId, string contactId)
    {
        var contact = await _contactRepository.GetAll()
            .Where(c => c.UserId == userId && c.Id == contactId)
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

    public async Task CreateContact(string userId, CreateContactDto contactDto)
    {
        var address = Address.Create(contactDto.StreetNumber, contactDto.StreetName, contactDto.City,
            contactDto.State, contactDto.Country);
        await _addressRepository.Add(address);

        var contact = Contact.Create(contactDto.Name, userId, contactDto.PhoneNumber, contactDto.EmailAddress,
            address.Id);
        await _contactRepository.Add(contact);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Result<SingleContactDto>> UpdateContact(string userId, string contactId,
        UpdateContactDto contactDto)
    {
        var contact = await _contactRepository.GetAll()
            .Where(c => c.UserId == userId && c.Id == contactId)
            .Include(c => c.Address)
            .FirstOrDefaultAsync();

        if (contact is null)
            return new Error[] { new("Contact.Error", "Contact not found") };

        contact.Name = contactDto.Name ?? contact.Name;
        contact.PhoneNumber = contactDto.PhoneNumber ?? contact.PhoneNumber;
        contact.EmailAddress = contactDto.EmailAddress ?? contact.EmailAddress;
        contact.Address.Number = contactDto.StreetNumber ?? contact.Address.Number;
        contact.Address.Street = contactDto.StreetName ?? contact.Address.Street;
        contact.Address.City = contactDto.City ?? contact.Address.City;
        contact.Address.State = contactDto.State ?? contact.Address.State;
        contact.Address.Country = contactDto.Country ?? contact.Address.Country;

        await _unitOfWork.SaveChangesAsync();

        return new SingleContactDto(
            contact.Name,
            contact.PhoneNumber,
            $"{contact.Address.Number} {contact.Address.Street}, {contact.Address.City}, {contact.Address.State}, {contact.Address.Country}");
    }

    public async Task<Result> DeleteContact(string userId, string contactId)
    {
        var contact = await _contactRepository.FindById(contactId);

        if (contact is null)
            return new Error[] { new("Contact.Error", "Contact not found") };

        if (contact.UserId != userId)
            return new Error[] { new("Contact.Error", "Contact not found") };

        _contactRepository.Remove(contact);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}