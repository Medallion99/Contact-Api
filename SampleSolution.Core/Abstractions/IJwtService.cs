using SampleSolution.Domain.Entities;

namespace SampleSolution.Core.Abstractions;

public interface IJwtService
{
    public string GenerateToken(User user);
}