namespace SampleSolution.Domain.Entities;

public class Address : Entity, IAuditable
{
    public string Number { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static Address Create(string number, string street, string city, string state, string country)
    {
        return new Address
        {
            Number = number,
            Street = street,
            City = city,
            State = state,
            Country = country
        };
    }
}