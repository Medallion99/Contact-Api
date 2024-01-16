namespace SampleSolution.Domain.Entities;

public class Contact : Entity, IAuditable
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public string AddressId { get; set; }

    public User User { get; set; }
    public Address Address { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static Contact Create(string name, string userId, string phoneNumber, string emailAddress, string addressId)
    {
        return new Contact
        {
            Name = name,
            UserId = userId,
            PhoneNumber = phoneNumber,
            EmailAddress = emailAddress,
            AddressId = addressId
        };
    }
}