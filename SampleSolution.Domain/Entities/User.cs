using Microsoft.AspNetCore.Identity;

namespace SampleSolution.Domain.Entities;

public class User : IdentityUser, IAuditable
{
    public string Name { get; set; }
    public IEnumerable<Contact> Contacts { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static User Create(string name, string email)
    {
        return new User
        {
            Name = name,
            Email = email,
            UserName = email,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }
}