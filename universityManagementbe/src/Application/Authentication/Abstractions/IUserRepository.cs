using Domain.Entities;

namespace Application.Authentication.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}