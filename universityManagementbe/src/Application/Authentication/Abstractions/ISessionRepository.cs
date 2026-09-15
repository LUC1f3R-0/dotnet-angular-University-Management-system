using Domain.Entities;

namespace Application.Authentication.Abstractions;

public interface ISessionRepository
{
    Task AddAsync(Session session, CancellationToken cancellationToken = default);
}